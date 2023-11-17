
public enum EditorState {
    MainMenu = 0,
    Editor = 1,
    Quitting = 2
}

public enum TileType {
    None = 0,
    Straight = 1,
    Corner = 2,
    Start = 3,
    End = 4,
    Tunnel = 5,
    Train_Station = 6,
    Water = 7,
    Bridge = 8,
    Switch_Left_Right = 9,
    Switch_Straight_Left = 10,
    Switch_Straight_Right = 11
}

public enum TileRotation {
    Zero = 0,
    One_Fourth = 1,
    Half = 2,
    Three_Fourth = 3
}

public enum SwitchState {
    None = 0,
    One = 1,
    Two = 2
}

public enum SwitchInputType {
    None = 0,
    Green = 1,
    Yellow = 2,
    Blue = 3,
    Red = 4
}