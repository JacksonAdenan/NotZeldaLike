using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level
{
	public int levelWidth;
	public int levelHeight;
	public Room[,] grid;// = new Room[levelWidth, levelHeight];


	public Room entranceRoom;
	public Room exitRoom;

	private Room currentRoom = null;
	private Room previousRoom = null;
	public Level(int width, int height)
	{
		levelWidth = width;
		levelHeight = height;

		grid = new Room[levelWidth, levelHeight];
	}


	public void InitRooms()
	{
		

		for (int i = 0; i < levelWidth; i++)
		{
			for (int j = 0; j < levelHeight; j++)
			{
				grid[i, j] = new Room();
				grid[i, j].x = i;
				grid[i, j].y = j;
			}
		}
	}

	public void GenerateMainPath()
	{
		GenerateEntrance();
		PickNextRoom();
	}
	public void GenerateEntrance()
	{
		int entranceRoomNumber = Random.Range(0, levelWidth);

		grid[entranceRoomNumber, 0].type = RoomType.Entrance;
		Debug.Log("Entrance room is " + "X: " + entranceRoomNumber + " Y: " + 0);


		currentRoom = grid[entranceRoomNumber, 0];
		entranceRoom = currentRoom;
	}

	// PickNextRoom() is poorly named. It will create a path by randomly selecting left, right or down until it hits the bottom of the grid. Once at the bottom it will create an exit room if it tries to go down
	// one more time.
	public void PickNextRoom()
	{

		
		// 0 -> Right
		// 1 -> Downwards
		// 2 -> Left

		while (currentRoom.type != RoomType.Exit)
		{
			Debug.Log("Current Room: " + "Type: " + currentRoom.type.ToString() + ", Pattern: " + currentRoom.pattern.ToString());

			int nextRoomNum = Random.Range(0, 3);
			if (nextRoomNum == 0)
			{
				previousRoom = currentRoom;

				if (previousRoom.pattern == RoomPattern.Right || previousRoom.pattern == RoomPattern.LeftRight)
				{
					continue;
				}

				if (currentRoom.x != 3)
				{
					currentRoom = grid[currentRoom.x + 1, currentRoom.y];
					currentRoom.pattern = RoomPattern.Left;
					previousRoom.AddRightRoom();
				}
				else
				{
					// If we hit this, we have hit the right wall already and are trying to go right one more time. In this case we add a down connection to this room and go down one room.
					currentRoom.AddDownRoom();

					previousRoom = currentRoom;
					currentRoom = grid[currentRoom.x, currentRoom.y + 1];
					currentRoom.pattern = RoomPattern.Up;
					if (currentRoom.y == 3)
					{ 
						currentRoom.type = RoomType.Exit;
						exitRoom = currentRoom;
					}
				}
			}
			else if (nextRoomNum == 2)
			{
				previousRoom = currentRoom;

				if (previousRoom.pattern == RoomPattern.Left || previousRoom.pattern == RoomPattern.LeftRight)
				{
					continue;
				}

				if (currentRoom.x != 0)
				{ 
					currentRoom = grid[currentRoom.x - 1, currentRoom.y];
					currentRoom.pattern = RoomPattern.Right;
					previousRoom.AddLeftRoom();
				}
				else
				{
					// If we hit this, we have hit the left wall already and are trying to go left one more time. In this case we add a down connection to this room and go down one room.
					currentRoom.AddDownRoom();

					previousRoom = currentRoom;
					currentRoom = grid[currentRoom.x, currentRoom.y + 1];
					currentRoom.pattern = RoomPattern.Up;
					if (currentRoom.y == 3)
					{ 
						currentRoom.type = RoomType.Exit;
						exitRoom = currentRoom;
					}
				}
			}
			else if (nextRoomNum == 1)
			{
				previousRoom = currentRoom;
				currentRoom = grid[currentRoom.x, currentRoom.y + 1];
				// We found the exit room.
				if (currentRoom.y == 3)
				{
					currentRoom.pattern = RoomPattern.Up;
					previousRoom.AddDownRoom();
					currentRoom.type = RoomType.Exit;
					exitRoom = currentRoom;
				}
				else
				{
					currentRoom.pattern = RoomPattern.Up;
					previousRoom.AddDownRoom();
				}
				 
				
			}
		}
		
	}


	// GenerateDeadEnds() is designed to be run after PickNextRoom(). It will go over the grid again selecting appropriate rooms to add "dead-end" rooms to.
	// !!Warning!! reallllly long switch statement ahead! I couldn't think of a better way :(
	public void GenerateDeadEnds()
	{
		for (int i = 0; i < levelWidth; i++)
		{
			for (int j = 0; j < levelHeight; j++)
			{
				Room currentRoom = grid[i, j];
				if (grid[i, j].pattern != RoomPattern.Closed && currentRoom.type != RoomType.DeadEnd && currentRoom.type != RoomType.Entrance && currentRoom.type != RoomType.Exit)
				{
					switch (currentRoom.pattern)
					{
						case RoomPattern.UpDown:
							{ 
								int randomNum = Random.Range(0, 2);
								if (randomNum == 0 && currentRoom.x != 0)
								{
									currentRoom.AddLeftRoom();
									//grid[i - 1, j].pattern = RoomPattern.Right;
									grid[i - 1, j].AddRightRoom();
									grid[i - 1, j].type = RoomType.DeadEnd;
								}
								else if (randomNum == 1 && currentRoom.x != 3)
								{ 
									currentRoom.AddRightRoom();
									//grid[i + 1, j].pattern = RoomPattern.Left;
									grid[i + 1, j].AddLeftRoom();
									grid[i + 1, j].type = RoomType.DeadEnd;
								}
								break;
							}
						case RoomPattern.RightUp:
							{ 
								int randomNum = Random.Range(0, 2);
								if (randomNum == 0 && currentRoom.x != 0)
								{
									currentRoom.AddLeftRoom();
									//grid[i - 1, j].pattern = RoomPattern.Right;
									grid[i - 1, j].AddRightRoom();
									grid[i - 1, j].type = RoomType.DeadEnd;
								}
								else if (randomNum == 1 && currentRoom.y != 3)
								{
									currentRoom.AddDownRoom();
									//grid[i, j - 1].pattern = RoomPattern.Up;
									grid[i, j - 1].AddUpRoom();
									grid[i, j - 1].type = RoomType.DeadEnd;
								}
								break;
							}
						case RoomPattern.RightDown:
							{
								int randomNum = Random.Range(0, 2);
								if (randomNum == 0 && currentRoom.x != 0)
								{
									currentRoom.AddLeftRoom();
									//grid[i - 1, j].pattern = RoomPattern.Right;
									grid[i - 1, j].AddRightRoom();
									grid[i - 1, j].type = RoomType.DeadEnd;
								}
								else if (randomNum == 1 && currentRoom.y != 0)
								{
									currentRoom.AddUpRoom();
									//grid[i, j + 1].pattern = RoomPattern.Down;
									grid[i, j + 1].AddDownRoom();
									grid[i, j + 1].type = RoomType.DeadEnd;
								}
								break;
							}
						case RoomPattern.LeftRight:
							{
								int randomNum = Random.Range(0, 2);
								if (randomNum == 0 && currentRoom.y != 0)
								{
									currentRoom.AddUpRoom();
									//grid[i, j + 1].pattern = RoomPattern.Down;
									grid[i, j - 1].AddDownRoom();
									grid[i, j - 1].type = RoomType.DeadEnd;
								}
								else if (randomNum == 1 && currentRoom.y != 3)
								{
									currentRoom.AddDownRoom();
									//grid[i, j - 1].pattern = RoomPattern.Up;
									grid[i, j + 1].AddUpRoom();
									grid[i, j + 1].type = RoomType.DeadEnd;
								}
								break;
							}
						case RoomPattern.LeftUp:
							{
								int randomNum = Random.Range(0, 2);
								if (randomNum == 0 && currentRoom.x != 3)
								{
									currentRoom.AddRightRoom();
									//grid[i + 1, j].pattern = RoomPattern.Left;
									grid[i + 1, j].AddLeftRoom();
									grid[i + 1, j].type = RoomType.DeadEnd;
								}
								else if (randomNum == 1 && currentRoom.y != 3)
								{
									currentRoom.AddDownRoom();
									//grid[i, j - 1].pattern = RoomPattern.Up;
									grid[i, j - 1].AddUpRoom();
									grid[i, j - 1].type = RoomType.DeadEnd;
								}
								break;
							}
						case RoomPattern.LeftDown:
							{
								int randomNum = Random.Range(0, 2);
								if (randomNum == 0 && currentRoom.x != 3)
								{
									currentRoom.AddRightRoom();
									//grid[i + 1, j].pattern = RoomPattern.Left;
									grid[i + 1, j].AddLeftRoom();
									grid[i + 1, j].type = RoomType.DeadEnd;
								}
								else if (randomNum == 1 && currentRoom.y != 0)
								{
									currentRoom.AddUpRoom();
									//grid[i, j + 1].pattern = RoomPattern.Down;
									grid[i, j + 1].AddDownRoom();
									grid[i, j + 1].type = RoomType.DeadEnd;
								}
								break;
							}
					}
				}
			}
		}
	}
}
