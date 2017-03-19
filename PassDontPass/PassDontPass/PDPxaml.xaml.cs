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
        }

        void OnBetButtonClicked(object sender, EventArgs args)
        {
            Button btnBetButton = (Button)sender;

            string strCurrentBet = betAmountLabel.Text;
            strCurrentBet = strCurrentBet.Substring(0, strCurrentBet.Length - 3); // drop the decimal and ending double zero
            strCurrentBet = strCurrentBet.Replace("$","").Replace(" ",""); // drop the dollar sign and leading space

            string strCurrentBankroll = bankrollAmountLabel.Text;
            strCurrentBankroll = strCurrentBankroll.Substring(0, strCurrentBankroll.Length - 3); // drop the decimal and ending double zero
            strCurrentBankroll = strCurrentBankroll.Replace("$", "").Replace(" ", "").Replace(",",""); // drop the dollar sign, leading space, and any comma

            int intCurrentBet = Convert.ToInt32(strCurrentBet);
            int intCurrentBankroll = Convert.ToInt32(strCurrentBankroll);
            int intBetChange = 0;

            if(btnBetButton.Text.Substring(0,1) == "+")
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

            betAmountLabel.Text = "$ " + intCurrentBet.ToString() + ".00";
            bankrollAmountLabel.Text = "$ " + intCurrentBankroll.ToString() + ".00";

            rollStatusLabel.Text = "Update bet or roll the dice.";
        }

        void OnRollButtonClicked(object sender, EventArgs args)
        {
            // generate the die vallues rolled
            Random rndm = new Random();
            int intDie1 = rndm.Next(1,7);
            int intDie2 = rndm.Next(1, 7);
            int intRollTotal = intDie1 + intDie2;

            // display the dice images
            string strDieImageSource = "PassDontPass.Images." + intDie1.ToString() + ".jpg";

            //imgDie1.Source = strDieImageSource;
            imgDie1.Source = ImageSource.FromResource(strDieImageSource);

            strDieImageSource = "PassDontPass.Images." + intDie2.ToString() + ".jpg";
            imgDie2.Source = ImageSource.FromResource(strDieImageSource);

            rollValueLabel.Text = intRollTotal.ToString();
        }

        void OnBetTypeButtonClicked(object sender, EventArgs args)
        {
            Button btnBetType = (Button)sender;
            string strBetType = btnBetType.Text.Substring(0, 1);

            if(strBetType == "P")
            {
                betTypeLabel.Text = "Pass";
            }
            else
            {
                betTypeLabel.Text = "Don't pass";
            }
        }
    }
}
