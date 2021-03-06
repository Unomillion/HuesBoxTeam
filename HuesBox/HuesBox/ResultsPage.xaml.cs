﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HuesBox
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResultsPage : ContentPage
	{
        public ObservableCollection<ColorOutput> Compliments { get; set; }
        public double NewRed { get; set; } = 0;
        public double NewBlue { get; set; } = 0;
        public double NewGreen { get; set; } = 0;
        public string NewHexRed { get; set; } = "00";
        public string NewHexBlue { get; set; } = "00";
        public string NewHexGreen { get; set; } = "00";
        public string ExportColor1 { get; set; } = "#FFFFFF";
        public string ExportColor2 { get; set; } = "#FFFFFF";
        public string ExportColor3 { get; set; } = "#FFFFFF";
        public string ExportColor4 { get; set; } = "#FFFFFF";
        public string ExportColor5 { get; set; } = "#FFFFFF";

        public ResultsPage(String UserInput, String HexValueRed, String HexValueBlue, String HexValueGreen, Double ColorRed, Double ColorBlue, Double ColorGreen)
   {
            
			InitializeComponent ();
            UserInputLabel.Text = "#" + UserInput;
            UserInputBoxView.BackgroundColor = Color.FromHex(UserInput);
            this.Compliments = new ObservableCollection<ColorOutput>();
            NewRed = 255 - ColorRed;
            NewBlue = 255 - ColorBlue;
            NewGreen = 255 - ColorGreen;
            
            NewHexRed = Convert.ToInt32(NewRed).ToString("X2");
            NewHexBlue = Convert.ToInt32(NewBlue).ToString("X2");
            NewHexGreen = Convert.ToInt32(NewGreen).ToString("X2");

            ExportColor1 = "#" + NewHexRed + NewHexGreen + NewHexBlue;


            this.Compliments.Add(new ColorOutput { HexColor = Color.FromHex(NewHexRed + NewHexGreen + NewHexBlue), HexValue = "# " + NewHexRed + NewHexGreen + NewHexBlue});
            for(int i = 0; i < 5; i++)
            {
                String HSLHex = RGBtoHSL(ColorRed, ColorGreen, ColorBlue, i);
                this.Compliments.Add(new ColorOutput { HexColor = Color.FromHex(HSLHex), HexValue = "#" + HSLHex });
 
                if (i == 0)
                    { ExportColor2 = "#" + HSLHex; };
                if (i == 1)
                    { ExportColor3 = "#" + HSLHex; };
                if (i == 2)
                    { ExportColor4 = "#" + HSLHex; };
                if (i == 3)
                    { ExportColor5 = "#" + HSLHex; };
            }
            

            

            ResultsListView.ItemsSource = this.Compliments;
		}

        public class ColorOutput
        {
            public Color HexColor { get; set; }
            public String HexValue { get; set; }
        }
        public static String RGBtoHSL(double ColorRed, double ColorGreen, double ColorBlue, int i)
        {
            double Light = 0;
            double Hue = 0;
            double Hue2 = 0;
            double Saturation = 0;
            double NewHSLRed = 0;
            double NewHSLBlue = 0;
            double NewHSLGreen = 0;
            double HSLRed = ColorRed / 255f;
            double HSLBlue = ColorBlue / 255f;
            double HSLGreen = ColorGreen / 255f;
            double Red = 0;
            double Blue = 0;
            double Green = 0;
            double Var1 = 0;
            double Var2 = 0;
            string HexRed = "";
            string HexBlue = "";
            string HexGreen = "";
            double[] HSLArray = { HSLRed, HSLBlue, HSLGreen };

            double ValMin = HSLArray.Min();
            double ValMax = HSLArray.Max();
            double ValRange = ValMax - ValMin;

            Light = (ValMax + ValMin) / 2f;
            if (ValRange == 0)
            {
                Hue = 0;
                Saturation = 0;
            }
            else
            {
                if (Light < 0.5)
                {
                    Saturation = ValRange / (ValMax + ValMin);

                }
                else
                {
                    Saturation = ValRange / (2 - ValMax - ValMin);
                }
                NewHSLRed = (((ValMax - HSLRed) / 6f) + (ValRange / 2f)) / ValRange;
                NewHSLBlue = (((ValMax - HSLBlue) / 6f) + (ValRange / 2f)) / ValRange;
                NewHSLGreen = (((ValMax - HSLGreen) / 6f) + (ValRange / 2f)) / ValRange;

                if (HSLRed == ValMax)
                {
                    Hue = NewHSLBlue - NewHSLGreen;
                }
                else if (HSLGreen == ValMax)
                {
                    Hue = (1f / 3f) + NewHSLRed - NewHSLBlue;
                }
                else if (HSLBlue == ValMax)
                {
                    Hue = (2f / 3f) + NewHSLGreen - NewHSLRed;
                }
                
                if (Hue < 0)
                {
                    Hue += 1;
                }

                if (Hue > 1)
                {
                    Hue -= 1;
                }

                
            }

            Hue2 = Hue + .5 + i * (1f/50f);
            if (Hue2 > 1)
            {
                Hue2 -= 1;
            }

            if (Saturation == 0)
            {
                Red = Light * 255;
                Green = Light * 255;
                Blue = Light * 255;
            }
            else
            {
                if (Light < 0.5)
                {
                    Var2 = Light * (1 + Saturation);
                }
                else
                {
                    Var2 = (Light + Saturation) - (Saturation * Light);
                }
                Var1 = 2 * Light - Var2;
                Red = 255 * HuetoRGB(Var1, Var2, (Hue2 + (1f / 3f)));
                Green = 255 * HuetoRGB(Var1, Var2, Hue2);
                Blue = 255 * HuetoRGB(Var1, Var2, Hue2 - (1f / 3f));

            }
            HexRed = Convert.ToInt32(Red).ToString("X2");
            HexBlue = Convert.ToInt32(Blue).ToString("X2");
            HexGreen = Convert.ToInt32(Green).ToString("X2");

            return (HexRed + HexGreen + HexBlue);
        }

        public static double HuetoRGB (double Var1, double Var2, double Hue)
        {
            if (Hue < 0)
            {
                Hue += 1;
            }

            if (Hue > 1)
            {
                Hue -= 1;
            }

            if ((6 * Hue) < 1)
            {
                return (Var1 + (Var2 - Var1) * 6 * Hue);
            }
            if ((2 * Hue) < 1)
            {
                return Var2;
            }
            if ((3 * Hue) < 2)
            {
                return (Var1 + (Var2 - Var1) * ((2f / 3f - Hue) * 6));
            }
            return Var1;
        }




        private void ExportButton_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ExportPage(ExportColor1, ExportColor2, ExportColor3, ExportColor4, ExportColor5));
        }

    }
}

