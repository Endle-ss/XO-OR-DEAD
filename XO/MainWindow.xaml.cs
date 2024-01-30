using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
namespace XO
{
    public partial class MainWindow : Window
    {
        private List<Button> availableButtons;
        private bool playerXTurn = true; // true - ходит игрок X, false - ходит игрок O
        private bool gameOver = false;
        private bool userStarts = true; // Определяет, начинает ли игру пользователь (X) или робот (O)
        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }
        private void InitializeGame()
        {
            availableButtons = new List<Button> { Butt1, Butt2, Butt3, Butt4, Butt5, Butt6, Butt7, Butt8, Butt9 };
            foreach (var button in availableButtons)
            {
                button.IsEnabled = true;
                button.Content = "";
            }
            playerXTurn = userStarts;
            gameOver = false;
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (gameOver)
                return;
            Button button = (Button)sender;
            if (button.Content == "")
            {
                button.Content = playerXTurn ? "X" : "O";
                CheckForWinner();
                if (!gameOver)
                    NextMove();
            }
        }
        private void NextMove()
        {
            playerXTurn = !playerXTurn;

            if (gameOver)
                return;

            if (playerXTurn)
            {
                Text.Text = "Ваш ход";
            }
            else
            {
                RobotMove();
            }
        }
        private void CheckForWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                if (CheckThreeInRow(GetButtonByName("Butt" + (i * 3 + 1)), GetButtonByName("Butt" + (i * 3 + 2)), GetButtonByName("Butt" + (i * 3 + 3))))
                    return;
                if (CheckThreeInRow(GetButtonByName("Butt" + (i + 1)), GetButtonByName("Butt" + (i + 4)), GetButtonByName("Butt" + (i + 7))))
                    return;
            }
            if (CheckThreeInRow(GetButtonByName("Butt1"), GetButtonByName("Butt5"), GetButtonByName("Butt9")))
                return;
            if (CheckThreeInRow(GetButtonByName("Butt3"), GetButtonByName("Butt5"), GetButtonByName("Butt7")))
                return;
            bool allButtonsFilled = availableButtons.All(button => button.Content.ToString() != "");
            if (allButtonsFilled && !gameOver)
            {
                DisplayResult("Ничья!");
                gameOver = true;
                return;
            }
        }
        private bool CheckThreeInRow(Button button1, Button button2, Button button3)
        {
            if (button1.Content != "" && button1.Content == button2.Content && button2.Content == button3.Content)
            {
                string winner = playerXTurn ? "X" : "O";
                DisplayResult("Победил игрок: " + winner + "!");
                gameOver = true;
                LockButtons();
                return true;
            }
            return false;
        }
        private Button GetButtonByName(string name)
        {
            return availableButtons.Find(button => button.Name == name);
        }
        private void DisplayResult(string result)
        {
            MessageBox.Show(result, "Результат игры");
            gameOver = true;
        }
        private void RobotMove()
        {
            if (availableButtons.Count > 0)
            {
                List<Button> emptyButtons = availableButtons.FindAll(button => button.Content.ToString() == "");
                if (emptyButtons.Count > 0)
                {
                    Random random = new Random();
                    int randomIndex = random.Next(emptyButtons.Count);
                    Button robotMove = emptyButtons[randomIndex];
                    robotMove.Content = "O";
                    CheckForWinner();
                    NextMove(); 
                }
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            userStarts = !userStarts; 
            InitializeGame();
            if (!userStarts)
            {
                RobotMove();
            }
        }
        private void LockButtons()
        {
            foreach (var button in availableButtons)
            {
                button.IsEnabled = false;
            }
        }
    }
}
