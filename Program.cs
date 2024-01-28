using System;
using System.Linq;
class Game
{
    const int ROW = 6;
    const int COL=7;
    const int PLYER1 = 1;
    const int PLYER2 = 2;
    const double PositiveInfinity = double.PositiveInfinity;
    const double NegativeInfinity = double.NegativeInfinity;
    public static int[,] board = new int[ROW,COL];
    public static int []emptySpace ={5,5,5,5,5,5,5};

    public static Random random = new Random();


    static void Main()
    {

        PlayerVSAI();


    }
    static void PlayerVSAI(){
        bool endGame = false;
        int moves = 0;
        bool plyerMove =true;
        while(moves<42&&!endGame){
            PrintArray(board);
            if(plyerMove){
                PlyerMove(PLYER1);
                endGame =Winning(PLYER1,board);
                plyerMove = !plyerMove;
            }else{
                int col = MiniMaxTree(board,emptySpace,3,NegativeInfinity,PositiveInfinity).Item2;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("AI enter a piece to "+col);
                Console.ResetColor();
                AddPieces(col,PLYER2);
                endGame =Winning(PLYER2,board);
                plyerMove = !plyerMove;
            }
            moves++;
        }
        PrintArray(board);
        if(moves==42){
            Console.WriteLine("its a tie!!!");
        }else{
            if(plyerMove) Console.WriteLine("player2 win!!!"); else Console.WriteLine("player1 win!!!");
        }
    }
    static void PlyerVSPlyer(){
        bool endGame = false;
        int moves = 0;
        bool turn =true;
        while(moves<42&&!endGame){
            PrintArray(board);
            if(turn){
                PlyerMove(PLYER1);
                endGame =Winning(PLYER1,board);
                turn = !turn;
            }else{
                PlyerMove(PLYER2);
                endGame =Winning(PLYER2,board);
                turn = !turn;
            }
            moves++;
        }
        PrintArray(board);
        if(moves==42){
            Console.WriteLine("its a tie!!!");
        }else{
            if(turn) Console.WriteLine("player2 win!!!"); else Console.WriteLine("player1 win!!!");
        }
    }
    static void PlyerMove(int player){
        int col=0;
            bool input = true;
            if (player==1){
                Console.ForegroundColor = ConsoleColor.Yellow;
            }else{
                Console.ForegroundColor = ConsoleColor.Blue;
            }

            Console.WriteLine("player " +player +" enter a piece to the board: ");
            while(input){            
                try
                {
                    col = Convert.ToInt32(Console.ReadLine());
                    if (col<0 || col>7){
                        throw new ArithmeticException("not in range");
                    }else{
                        if(emptySpace[col]<0){
                            throw new ArithmeticException("no space in this column");
                        }else input =false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.ResetColor();
            AddPieces(col,player);
    }
    static void AddPieces(int col,int plyer){
        if (col>6||col<0){
            Console.WriteLine("not in range");
            return;
        }
        if (emptySpace[col]<0){
            Console.WriteLine("no space in this culomn");
            return;
        }
        board[emptySpace[col],col]=plyer;
        UpdateEmptySpace(col);
    }
    static double Heuristics(int[,] board, int plyer){
        int heuristics =0;
        int [] arr={0,0,0,0};
        for(int c=0; c < COL; c++){
            for( int r=5 ; r >= ROW-3;r--){
                arr[0]=board[r,c];
                arr[1]=board[r-1,c];
                arr[2]=board[r-2,c];
                arr[3]=board[r-3,c];
                heuristics+=PositionScore(arr,plyer);
            }
        }
        //horizontal
        for(int c=0; c < COL-3; c++){
            for( int r=5 ; r>=0;r--){
                arr[0]=board[r,c];
                arr[1]=board[r,c+1];
                arr[2]=board[r,c+2];
                arr[3]=board[r,c+3];
            }
        }
        //Ascending diagonal
        for(int c=0; c < COL-3; c++){
            for( int r=5 ; r>= ROW-3;r--){
                arr[0]=board[r,c];
                arr[1]=board[r-1,c+1];
                arr[2]=board[r-2,c+2];
                arr[3]=board[r-3,c+3];
                heuristics+=PositionScore(arr,plyer);
            }
        }
        //Descending diagonal
        for(int c=3; c < COL; c++){
            for( int r=5 ; r>=ROW-3;r--){
                arr[0]=board[r,c];
                arr[1]=board[r-1,c-1];
                arr[2]=board[r-2,c-2];
                arr[3]=board[r-3,c-3];
                heuristics+=PositionScore(arr,plyer);
            }
        }
        return heuristics;
    }
    static int PositionScore(int[] position, int plyer){
        int num = 4- position.Count(x=>x==0);
        if ((position.Contains(PLYER2)&&position.Contains(PLYER1))||num==0) return 0;
        var score = num switch
        {
            1 => 1,
            2 => 10,
            3 => 100,
            4 => 1000,
            _ => 0,
        };
        return position.Contains(plyer)? score: score*-1;
    }
    static bool Winning(int plyer, int[,]board){
        //vertical
        for(int c=0; c < COL; c++){
            for( int r=5 ; r >= ROW-3;r--){
                if (board[r,c]==0){break;}
                if(board[r,c]==plyer&&board[r-1,c]==plyer&&board[r-2,c]==plyer&&board[r-3,c]==plyer){return true;}
            }
        }
        //horizontal
        for(int c=0; c < COL-3; c++){
            for( int r=5 ; r>=0;r--){
                if (board[r,c]==0){break;}
                if(board[r,c]==plyer && board[r,c+1]==plyer && board[r,c+2]==plyer && board[r,c+3]==plyer){return true;}
            }
        }
        //Ascending diagonal
        for(int c=0; c < COL-3; c++){
            for( int r=5 ; r>= ROW-3;r--){
                if (board[r,c]==0){break;}
                if(board[r,c]==plyer&&board[r-1,c+1]==plyer&&board[r-2,c+2]==plyer&&board[r-3,c+3]==plyer){return true;}
            }
        }
        //Descending diagonal
        for(int c=3; c < COL; c++){
            for( int r=5 ; r>=ROW-3;r--){
                if (board[r,c]==0){break;}
                if(board[r,c]==plyer&&board[r-1,c-1]==plyer&&board[r-2,c-2]==plyer&&board[r-3,c-3]==plyer){return true;}
            }
        }
        return false;
    }
    static void UpdateEmptySpace(int col) {emptySpace[col]-=1;}
    static int[] FreeColumns(int[] arr){
        int lan = arr.Count(x => x>=0);
        int[] temp = new int[lan];
        for(int i=0,x=0 ;i<arr.Length; i++){
            if(arr[i]>=0){
                temp[x]=i;
                x++;
            }
        }
        return temp;
    }
    static bool[] TerminalNode(int[,] board,int[] freeColumn){
        bool player1Win=Winning(PLYER1,board);
        bool player2win= Winning(PLYER2,board);
        bool noSpace= freeColumn.Length==0;
        return new bool[]{player1Win,player2win,noSpace} ;
    }
    static Tuple<double, int> MiniMaxTree(int[,] board,int[] emptySpace,int depth, double a, double b){
        int [] freeColumn = FreeColumns(emptySpace);
        bool[] terminal=TerminalNode(board,freeColumn);
        if(terminal.Contains(true)||depth==0){
            if(terminal[0])return Tuple.Create(NegativeInfinity,-1 );
            if(terminal[1])return Tuple.Create(PositiveInfinity,-1 );
            return Tuple.Create(Heuristics(board,PLYER2),-1);
        }     
        int[,] tempBoard = new int[board.GetLength(0), board.GetLength(1)];
        Array.Copy(board, tempBoard, board.Length);
        int[] tempEmptySpace = new int[emptySpace.Length];
        Array.Copy(emptySpace,tempEmptySpace,emptySpace.Length);
        int column = freeColumn[random.Next(freeColumn.Length)];

        if(depth%2==1){
            double value = NegativeInfinity;
            foreach(int col in freeColumn){
                int row = tempEmptySpace[col];
                tempEmptySpace[col]-=1;
                tempBoard[row,col]=PLYER2;
                double newScore = MiniMaxTree(tempBoard,tempEmptySpace,depth-1,a,b).Item1;
                tempEmptySpace[col]+=1;
                tempBoard[row,col]=0;
                if (newScore>value){
                    value=newScore;
                    column=col;
                }
                a = Math.Max(value,a);
                if(a>=b)break;
            }
            return Tuple.Create(value,column);
        }else{
            double value = PositiveInfinity;
            foreach(int col in freeColumn){
                int row = tempEmptySpace[col];
                tempEmptySpace[col]-=1;
                tempBoard[row,col]=PLYER1;
                double newScore = MiniMaxTree(tempBoard,tempEmptySpace,depth-1,a,b).Item1;
                tempEmptySpace[col]+=1;
                tempBoard[row,col]=0;
                if (newScore<value){
                    value=newScore;
                    column=col;
                }
                b = Math.Min(value,b);
                if(a>=b)break;
            }
            return Tuple.Create(value,column);
        }
    }

    // הדפסת המערך
    static void PrintArray(int[,] board)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            if (i==0)Console.WriteLine("  ___________________________");
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if(board[i,j]==1){
                    Console.Write(" | ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("O");
                    Console.ResetColor();
                }else if(board[i,j]==2){
                    Console.Write(" | ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("O");
                    Console.ResetColor();

                }else{
                    Console.Write(" |  ");
                }
            }
            Console.WriteLine(" |");
            Console.WriteLine(" |---|---|---|---|---|---|---|"); // שורה חדשה בסוף כל שורה
        }
    }
    static void PrintArray(int[] array){
        Console.WriteLine("Array elements:");
        foreach (int element in array){
            Console.Write(element + " ");
        }
        Console.WriteLine(); // שורה חדשה בסוף
    }
}
