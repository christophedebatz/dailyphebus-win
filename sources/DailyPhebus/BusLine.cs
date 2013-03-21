using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPhebus
{
    public class BusLine
    {
        public static string choice = "Choisissez dans la liste...";
        public string letter { get; private set; }
        public string firstExtremity { get; private set; }
        public string secondExtremity { get; private set; }

        public static BusLine Zero  = new BusLine("0", "0", "0");

        public BusLine (string letter, string firstExtremity, string secondExtremity)
        {
            this.letter = letter;
            this.firstExtremity = firstExtremity;
            this.secondExtremity = secondExtremity;
        }

        public string getExtremityByDirection(short direction)
        {
            return (direction == 1) ? this.firstExtremity : this.secondExtremity;
        }

        public override string ToString()
        {
            if (this.Equals(BusLine.Zero))
                return choice;
            else
                return this.letter + " (" + this.firstExtremity + " - " + this.secondExtremity + ")";
        }


        public static BusLine getLineByComboSelection (string selection)
        {
            if (selection.Equals(choice))
                return BusLine.Zero;

            string[] split1 = selection.Substring(0, selection.Length - 1).Split('(');
            string[] split2 = split1[1].Trim().Split('-');
            
            return new BusLine(split1[0].Trim(), split2[0].Trim(), split2[1].Trim());
        }
    }
}
