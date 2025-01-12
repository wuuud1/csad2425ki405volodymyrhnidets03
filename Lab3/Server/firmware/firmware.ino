const char SignCellX = 'x';
const char SignCellO = 'o';
const char SignEmptyCell = ' ';

struct Cell {
  int row;
  int col;
  char content;
};

struct Board {
  Cell Cells[3][3];

  void Init() {
    for (int i = 0; i < 3; i++) {
      for (int j = 0; j < 3; j++) {
        Cells[i][j].row = i;
        Cells[i][j].col = j;
        Cells[i][j].content = SignEmptyCell;
      }
    }
  }

  void SetCell(int row, int col, char content) {
    Cells[row][col].content = content;
  }

  char GetCell(int row, int col) {
    return Cells[row][col].content;
  }

  void CountSymbols(int &xCount, int &oCount) {
    xCount = 0;
    oCount = 0;
    for (int i = 0; i < 3; i++) {
      for (int j = 0; j < 3; j++) {
        if (Cells[i][j].content == SignCellX) {
          xCount++;
        } else if (Cells[i][j].content == SignCellO) {
          oCount++;
        }
      }
    }
  }

  Cell IsWinMoveExist(char player) {
    for (int i = 0; i < 3; i++) {
      for (int j = 0; j < 3; j++) {
        if (Cells[i][j].content == SignEmptyCell) {
          Cells[i][j].content = player;
          if (CheckWin(player)) {
            return Cells[i][j]; 
          }
          Cells[i][j].content = SignEmptyCell; 
        }
      }
    }
    return Cell{-1, -1, SignEmptyCell}; 
  }

  bool CheckWin(char player) {
    // Check rows, columns, and diagonals for a win
    for (int i = 0; i < 3; i++) {
      if (Cells[i][0].content == player && Cells[i][1].content == player && Cells[i][2].content == player)
        return true;
      if (Cells[0][i].content == player && Cells[1][i].content == player && Cells[2][i].content == player)
        return true;
    }
    if (Cells[0][0].content == player && Cells[1][1].content == player && Cells[2][2].content == player)
      return true;
    if (Cells[0][2].content == player && Cells[1][1].content == player && Cells[2][0].content == player)
      return true;

    return false;
  }

  String FindLine(char player) {
    for (int i = 0; i < 3; i++) {
      if ((Cells[i][0].content == player && Cells[i][1].content == player) || (Cells[i][1].content == player && Cells[i][2].content == player) || Cells[i][0].content == player && Cells[i][2].content == player)
        return "r";
    }
    for (int i = 0; i < 3; i++) {
      if ((Cells[0][i].content == player && Cells[1][i].content == player) || (Cells[1][i].content == player && Cells[2][i].content == player) || (Cells[0][i].content == player && Cells[2][i].content == player))
        return "c";
    }
    return "";
  }

  Cell RandFreeCell() {
    for(int i = 0; i<3;i++){
      for(int j =0; j<3;j++){
        if (Cells[i][j].content == SignEmptyCell) {
          return Cells[i][j];
      }
      }
    }
  }
};

Board board;
String receivedMessage = "";
int XNumber = 0;
int ONumber = 0;

void setup() {
  Serial.begin(9600);
}

void loop() {
  if (Serial.available() > 0) {
    receivedMessage = Serial.readString();

    if (receivedMessage.length() > 0) {
      board.Init();
      FillBoard();

      Serial.println(GenerateMove());
    }
  }

}

void FillBoard(){
  for (int i = 0; i < receivedMessage.length(); i += 3) {
        int row = receivedMessage[i] - '0';
        int col = receivedMessage[i + 1] - '0';
        char content = ToLowerCase(receivedMessage[i + 2]);

        board.SetCell(row, col, content);
      }

  board.CountSymbols(XNumber, ONumber);
}

char ToLowerCase(char c){
  if (c >= 'A' && c <= 'Z') {
            c = c + ('a' - 'A');
        }
  return c;
}

String GenerateMove(){
  if(XNumber == ONumber)
    return GenerateMoveX();

  return GenerateMoveO();
}

String GenerateMoveX(){
  String move = "";
 switch(XNumber){
  case 0:
    move = "00";
    break;
  case 1:
    move = MoveXStep1();
    break;
  case 2:
    move = MoveXStep2();
    break;
  case 3:
    move = MoveXStep3();
    break;
  default:
    Cell randomCell = board.RandFreeCell();
    move = String(randomCell.row)+String(randomCell.col);
    break;
  break;
 }

  return move;
}

String MoveXStep1(){
  String move = "02";
  if (!(board.GetCell(0, 1) == SignEmptyCell && board.GetCell(0, 2) == SignEmptyCell)) {
    move = "20";
  }

  return move;
}

String MoveXStep2(){
  String move = MoveWinNoLose(SignCellX);
  if(move != String(SignEmptyCell))
    return move;

  if(board.FindLine(SignCellX) == "r"){
    if(board.GetCell(1, 0) == SignEmptyCell && board.GetCell(2, 0) == SignEmptyCell)
      return  "20";
  }
  else{
    if(board.GetCell(0, 1) == SignEmptyCell && board.GetCell(0, 2) == SignEmptyCell)
      return "02";
  }

  return "22";
}

String MoveXStep3(){
  String move = MoveWinNoLose(SignCellX);

  if(move != String(SignEmptyCell))
    return move;

  Cell randMove = board.RandFreeCell();
  return String(randMove.row) + String(randMove.col);
}

String GenerateMoveO(){
  String move = MoveWinNoLose(SignCellO);
  if (move != String(SignEmptyCell)) {
    return move;
  }

  move = MoveWinNoLose(SignCellX);
  if (move != String(SignEmptyCell)) {
    return move;
  }

  if (board.GetCell(1, 1) == SignEmptyCell) {
    return "11"; // Center is prioritized
  }

  const int corners[4][2] = {{0, 0}, {0, 2}, {2, 0}, {2, 2}};
  for (int i = 0; i < 4; i++) {
    int row = corners[i][0], col = corners[i][1];
    if (board.GetCell(row, col) == SignEmptyCell) {
      return String(row) + String(col);
    }
  }

  const int edges[4][2] = {{0, 1}, {1, 0}, {1, 2}, {2, 1}};
  for (int i = 0; i < 4; i++) {
    int row = edges[i][0], col = edges[i][1];
    if (board.GetCell(row, col) == SignEmptyCell) {
      return String(row) + String(col);
    }
  }

  Cell randomCell = board.RandFreeCell();
  return String(randomCell.row) + String(randomCell.col);
}

String MoveWinNoLose(char player){
  Cell winMove = board.IsWinMoveExist(player);
  if (winMove.row != -1 && winMove.col != -1) 
    return String(winMove.row) + String(winMove.col);

  winMove = board.IsWinMoveExist(player == SignCellX? SignCellO: SignCellX);
  if (winMove.row != -1 && winMove.col != -1) 
    return String(winMove.row) + String(winMove.col);

  return String(SignEmptyCell);
}
