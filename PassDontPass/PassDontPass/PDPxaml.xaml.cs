using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PassDontPass
{
    public partial class PDPxaml : ContentPage
    {
        public PDPxaml()
        {
            InitializeComponent();
            Application.Current.Properties["RoundStatus"] = "ComeOut";
        }

        void OnBetButtonClicked(object sender, EventArgs args)
        {
            string strRoundStatus = Application.Current.Properties["RoundStatus"].ToString();
            if (strRoundStatus == "ComeOut" || strRoundStatus == "Starting")
            {
                Button btnBetButton = (Button)sender;

                int intCurrentBet = ConvertLabelValueToInt(betAmountLabel.Text);
                int intCurrentBankroll = ConvertLabelValueToInt(bankrollAmountLabel.Text);
                int intBetChange = 0;

                if (btnBetButton.Text.Substring(0, 1) == "+")
                {
                    intBetChange = 5;
                }
                else
                {
                    intBetChange = -5;
                }

                if (intCurrentBet + intBetChange <= 0)
                {
                    intBetChange = 0;
                    rollStatusLabel.Text = "Minimum bet is $ 5.00";
                }

                if (intCurrentBankroll + intBetChange <= 0)
                {
                    intBetChange = 0;
                    rollStatusLabel.Text = "Sorry, out of money!";
                }

                intCurrentBet += intBetChange;
                intCurrentBankroll -= intBetChange;

                betAmountLabel.Text = ConvertValueToLabel(intCurrentBet);
                bankrollAmountLabel.Text = ConvertValueToLabel(intCurrentBankroll);

                if (intCurrentBet > 0) { rollStatusLabel.Text = "Update bet or roll the dice."; }
            }
            else
            {
                rollStatusLabel.Text = "Can't change bet after come out roll.";
            }
        }

        void OnRollButtonClicked(object sender, EventArgs args)
        {
            bool booOKToRoll = false;
            if(ConvertLabelValueToInt(betAmountLabel.Text) > 0) { booOKToRoll = true; }

            if (booOKToRoll)
            {
                // generate the die vallues rolled
                Random rndm = new Random();
                int intDie1 = rndm.Next(1, 7);
                int intDie2 = rndm.Next(1, 7);
                int intRollTotal = intDie1 + intDie2;

                // display the dice images
                string strDieImageSource = "PassDontPass.Images." + intDie1.ToString() + ".jpg";
                imgDie1.Source = ImageSource.FromResource(strDieImageSource);

                strDieImageSource = "PassDontPass.Images." + intDie2.ToString() + ".jpg";
                imgDie2.Source = ImageSource.FromResource(strDieImageSource);

                rollValueLabel.Text = intRollTotal.ToString();

                /* ***************************************** **
                 * Determine action based on round status
                 * ***************************************** */
                string strRoundStatus = Application.Current.Properties["RoundStatus"].ToString();

                switch (strRoundStatus)
                {
                    case "ComeOut":
                        // win for shooter and pass bet
                        if (intRollTotal == 7 || intRollTotal == 11)
                        {
                            rollStatusLabel.Text = "Shooter wins.";
                            ResolveTheBet(true);
                        }
                        // craps, loss for shooter and pass bet
                        else if (intRollTotal == 2 || intRollTotal == 3 || intRollTotal == 12)
                        {
                            rollStatusLabel.Text = "Shooter crapped out.";
                            ResolveTheBet(false);
                        }
                        else
                        {
                            Application.Current.Properties["RoundStatus"] = "Point";

                            strDieImageSource = "PassDontPass.Images." + intDie1.ToString() + ".jpg";
                            imgPoint1.Source = ImageSource.FromResource(strDieImageSource);
                            strDieImageSource = "PassDontPass.Images." + intDie2.ToString() + ".jpg";
                            imgPoint2.Source = ImageSource.FromResource(strDieImageSource);

                            pointValueLabel.Text = intRollTotal.ToString();
                            rollStatusLabel.Text = "Point is " + intRollTotal.ToString() + ". Roll the dice.";
                        }
                        break;
                    case "Point":
                        int intPoint = Convert.ToInt32(pointValueLabel.Text);

                        // made the point, win for shooter and pass bets
                        if (intRollTotal == intPoint)
                        {
                            Application.Current.Properties["RoundStatus"] = "ComeOut";
                            ResolveTheBet(true);
                        }
                        // 7 before rolling the point
                        // loss for the shooter and pass bets
                        else if (intRollTotal == 7)
                        {
                            Application.Current.Properties["RoundStatus"] = "ComeOut";
                            ResolveTheBet(false);
                        }
                        else
                        {
                            rollStatusLabel.Text = "Roll again.";
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                rollStatusLabel.Text = "Please place a bet.";
            }
        }

        void OnBetTypeButtonClicked(object sender, EventArgs args)
        {
            string strRoundStatus = Application.Current.Properties["RoundStatus"].ToString();
            if (strRoundStatus == "ComeOut" || strRoundStatus == "Starting")
            {
                Button btnBetType = (Button)sender;
                string strBetType = btnBetType.Text.Substring(0, 1);

                if (strBetType == "P")
                {
                    betTypeLabel.Text = "Pass";
                }
                else
                {
                    betTypeLabel.Text = "Don't pass";
                }
            }
            else
            {
                rollStatusLabel.Text = "Can't change bet after come out roll.";
            }
        }

        void ResolveTheBet(bool booShooterWon)
        {
            bool booBettorWon = false;   // default is bettor lost
            string strBetType = betTypeLabel.Text.Substring(0, 1);
            string strFullBetType = betTypeLabel.Text;

            // change result to bettor won if conditions are met
            if ((strBetType == "P" && booShooterWon)    // shoter win and bet is pass
                || (strBetType == "D" && !booShooterWon))  // shooter losses and bet is don't pass
            {
                booBettorWon = true;
            }

            if(booBettorWon)
            {
                rollStatusLabel.Text = "You won your " + strFullBetType + " bet.";
                int intWonAmount = ConvertLabelValueToInt(betAmountLabel.Text);
                int intBankroll = ConvertLabelValueToInt(bankrollAmountLabel.Text) + intWonAmount;

                bankrollAmountLabel.Text = ConvertValueToLabel(intBankroll);
            }
            else
            {
                rollStatusLabel.Text = "You lost your " + strFullBetType + " bet.";
                betAmountLabel.Text = ConvertValueToLabel(0);
            }

            imgPoint1.Source = ImageSource.FromResource("PassDontPass.Images.0.jpg");
            imgPoint2.Source = ImageSource.FromResource("PassDontPass.Images.0.jpg");
        }
        
        public int ConvertLabelValueToInt(string strConvertMe)
        {
            strConvertMe = strConvertMe.Substring(0, strConvertMe.Length - 3); // drop the decimal and ending double zero
            strConvertMe = strConvertMe.Replace("$", "").Replace(" ", "").Replace(",",""); // drop the dollar sign and leading space

            return Convert.ToInt32(strConvertMe);
        }

        public string ConvertValueToLabel(int intConvertMe)
        {
            return "$ " + intConvertMe.ToString() + ".00";
        }
    }
}
