using UnityEngine;
using System.Collections;

public interface Room {

    void nullifyRoom();
    void CreateRoom();

    int getTile(int i, int j);

    int RoomWidth { get; set; }
    int RoomHeight { get; set; }
    int GridPosX { get; set; }
    int GridPosY { get; set; }
    bool IsNull { get; set; }
}
