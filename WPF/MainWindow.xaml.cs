using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BIB;
using System.Media;

namespace WPF
{
    
    public partial class MainWindow : Window
    {
        bool sound = true;
        SoundPlayer player;
        bool firstclick;
        DispatcherTimer Timer;
        int sec;
        int min;
        
        Button[,] bns;
        Game myGame;
        int lines;
        int columns;
        Level level = Level.Beginner;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void itemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void newGame_Click(object sender, RoutedEventArgs e)
        {
            firstclick = true;
            myCanvas.Children.Clear();
            sec = 0;
            min = 10;
           
            lbl.Content = "Timer: 00:" + min.ToString("00") + ":" + sec.ToString("00");
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += timer_Tick;
            
            myGame = new Game(level);
            bmbs.Content = "Bombs:" + (int)level;
            lines = myGame.Grid.GetLength(0);
            columns = myGame.Grid.GetLength(1);
            bns = new Button[lines, columns];
            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Button bt = new Button();
                    bt.Click += Bt_Click;
                    bt.MouseRightButtonDown += Bt_MouseRightButtonDown;
                    bt.Tag = i * 100 + j;
                    bt.Background = Brushes.GhostWhite;
                   // if (myGame.Grid[i, j] == 9)
                     //   bt.Content = 9;
                    bns[i, j] = bt;
                    myCanvas.Children.Add(bt);
                }
            }
            resizeLayouts();
        }

        private void Bt_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button b = (Button)sender;
            if (b.Content==null)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("flag.png", UriKind.Relative));
                StackPanel stackPnl = new StackPanel();
                stackPnl.Orientation = Orientation.Horizontal;
                stackPnl.Children.Add(img);
                b.Content = stackPnl;
                
            }
            else if(b.Background!=Brushes.LightCyan && b.Background!=Brushes.Red)
            {
                b.Content = null;
                
            }

        }

        private void Bt_Click(object sender, RoutedEventArgs e)
        {

            
            
                
                
            if(firstclick)
            {
                Timer.Start();
                firstclick = false;
            }
            Button b = (Button)sender;
            int l = (int)b.Tag / 100;
            int c = (int)b.Tag % 100;
            if(b.Content==null)
            {
                if (b.Background == Brushes.GhostWhite)
                {


                    if (myGame.Grid[l, c] == 0)
                    {
                        zero(l, c);
                        if (testwin())
                        {
                            showContent();
                            Timer.Stop();
                            SoundPlayer player3 = new SoundPlayer(Properties.Resources.app_14);
                            player3.Play();

                            if (MessageBox.Show("Victory!!! You Won!! your time is "+(9-min).ToString("0")+" min"+(60-sec).ToString("00")+" seconde", "Congrats", MessageBoxButton.OK) == MessageBoxResult.OK)
                            {
                                if (sound)
                                    player.Play();
                                else
                                    player3.Stop();


                                myCanvas.Children.Clear();
                            }

                        }
                    }
                    else if (myGame.Grid[l, c] == 9)
                    {
                        showContent();
                        Timer.Stop();
                        SoundPlayer player2 = new SoundPlayer(Properties.Resources.the_price_is_right_losing_horn);
                        player2.Play();
                        if (MessageBox.Show("Game Over!!!", "Sorry", MessageBoxButton.OK) == MessageBoxResult.OK)
                        {
                            if (sound)
                                player.Play();
                            else
                                player2.Stop();

                            myCanvas.Children.Clear();
                        }



                    }
                    else
                    {
                        b.Content = myGame.Grid[l, c];
                        b.Background = Brushes.LightCyan;
                        if (testwin())
                        {
                            showContent();
                            Timer.Stop();
                            SoundPlayer player3 = new SoundPlayer(Properties.Resources.app_14);
                            player3.Play();
                            if (MessageBox.Show("Victory!!! You Won!! your time is " + (9 - min).ToString("0") + " min" + (60 - sec).ToString("00") + " seconde", "Congrats", MessageBoxButton.OK) == MessageBoxResult.OK)
                            {
                                if (sound)

                                    player.Play();
                                else
                                    player3.Stop();

                                myCanvas.Children.Clear();
                            }
                        }
                    }


                }
            }
        }
            private void zero(int i, int j)
        {
            for (int x = i - 1; x <= i + 1; x++)
            {
                if (x > -1 && x < myGame.Grid.GetLength(0))
                    for (int y = j - 1; y <= j + 1; y++)
                    {
                        if (y > -1 && y < myGame.Grid.GetLength(1) && bns[x, y].Background == Brushes.GhostWhite)
                        {
                            bns[x, y].Background = Brushes.LightCyan;
                            bns[x, y].Content = myGame.Grid[x,y];
                            if(myGame.Grid[x,y]==0)
                                zero(x, y);
                        }
                    }
            }
        }

        private void lvl_b(object sender, RoutedEventArgs e)
        {
            level = Level.Beginner;
            newGame.Header = "New Game";
        }

        private void lvl_i(object sender, RoutedEventArgs e)
        {
            level = Level.Intermediate;
            newGame.Header = "New Game";
        }

        private void lvl_a(object sender, RoutedEventArgs e)
        {
            level = Level.Advanced;
            newGame.Header = "New Game";
        }
        private Boolean testwin()
        {
            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (myGame.Grid[i, j] != 9 && bns[i, j].Background == Brushes.GhostWhite)
                        return (false);
                }
            }
            return (true);
        }
        private void showContent()
        {
            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (myGame.Grid[i, j] == 9)
                    {
                        Image img = new Image();
                        img.Source = new BitmapImage(new Uri("bmb.png", UriKind.Relative));
                        StackPanel stackPnl = new StackPanel();
                        stackPnl.Orientation = Orientation.Horizontal;
                        stackPnl.Children.Add(img);
                        bns[i, j].Content = stackPnl;
                        bns[i, j].Background = Brushes.Red;
                    }
                    else
                    {
                        bns[i, j].Content = myGame.Grid[i, j];
                        bns[i, j].Background = Brushes.LightCyan;
                    }
                }
            }
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (sec == 0)
            {
                min--;
                sec = 59;
            }
            else
                sec--;
            lbl.Content = "Timer: 00:" + min.ToString("00") + ":" + sec.ToString("00");
            if(sec==0 && min==0)
            {
                Timer.Stop();
                showContent();
                SoundPlayer player2 = new SoundPlayer(Properties.Resources.the_price_is_right_losing_horn);
                player2.Play();
                if (MessageBox.Show("Time is over", "Sorry", MessageBoxButton.OK) == MessageBoxResult.OK)
                {
                    if (sound)
                        player.Play();
                    else
                        player2.Stop();
                    myCanvas.Children.Clear();
                }
            }
        }

        

        
        private void resizeLayouts()
        {
            myCanvas.Width = win.Width - 15;
            myCanvas.Height = win.Height - 80;
            myCanvas2.Width = win.Width - 50;
            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    bns[i, j].Width = myCanvas.Width / columns;
                    bns[i, j].Height = myCanvas.Height / lines;
                    Canvas.SetTop(bns[i, j], (double)i * bns[i, j].Height);
                    Canvas.SetLeft(bns[i, j], (double)j * bns[i, j].Width);
                }
            }
        }

        private void win_Loaded(object sender, RoutedEventArgs e)
        {
            player = new SoundPlayer(Properties.Resources.Benny_Hill_Theme_CBR_256k);
            player.Play();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked == true )
            {
                player.Play();
                sound = true;
               
            }
            else 
            {
                player.Stop();
                sound = false;
            }
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Game Created By:\nIdriss Mohamed\n", "About");
        }
    }
}
