/// <summary>
/// Game constants - Centralized configuration values
/// </summary>
public static class GameConstants
{
    // Board Configuration
    public const int BOARD_SIZE = 12;
    public const int QUAN_CELL_1 = 5;
    public const int QUAN_CELL_2 = 11;
    public const int PLAYER_CELLS_COUNT = 5;
    public const int PLAYER_1_START_INDEX = 0;
    public const int PLAYER_2_START_INDEX = 6;
    
    // Gameplay
    public const int INITIAL_STONES_PER_CELL = 5;
    public const int QUAN_SCORE = 10;
    public const int DAN_SCORE = 1;
    public const int DEBT_AMOUNT = 5;
    public const int INITIAL_QUAN_COUNT = 1;
    
    // Animation Timing
    public const float SOW_DELAY = 0.2f;
    public const float CAPTURE_DELAY = 0.2f;
    public const float SCALE_DURATION = 0.5f;
    public const float SCALE_FACTOR = 1.5f;
    public const float PULSE_DURATION = 0.3f;
    
    // AI Thinking Time
    public const int AI_MIN_THINK_TIME_MS = 500;
    public const int AI_MAX_THINK_TIME_MS = 1500;
    
    // Colors
    public const string COLOR_NORMAL = "#DBDBDB";
    public const string COLOR_SCORE_GAIN = "#FFD700";
    public const string COLOR_CAPTURE = "#FF6B6B";
    public const string COLOR_HIGHLIGHT = "#4ECDC4";
    
    // Player Indices
    public static class PlayerIndex
    {
        public const int PLAYER_1 = 0;
        public const int PLAYER_2 = 1;
    }
}
