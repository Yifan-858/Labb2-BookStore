using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminTools.Helper
{
    public class MenuHelper
    {
        public string Narrative;
        public List<string> Options;
        public int IndexSelected;

        public MenuHelper(string narrative, List<string> options)
        {
            Narrative = narrative;
            Options = options;
            IndexSelected = 0;
        }

        private void DisplayOptions()
        {
            for(int i=0; i<Options.Count; i++)
            {
                string optionSelected = Options[i];
                string pointer;

                if( i == IndexSelected)
                {
                    pointer = ">";
                }
                else
                {
                    pointer = " ";
                }
            
                Console.ResetColor();

                Console.WriteLine($"{pointer} {Options[i]}");
            }
        }

        public int ControlChoice()
        {
            ConsoleKey keyPressed ;

            do
            { 
                Console.Clear();

                Console.WriteLine(Narrative);
                Console.WriteLine();
                DisplayOptions();

                keyPressed = Console.ReadKey(true).Key;

                if(keyPressed == ConsoleKey.UpArrow)
                {
                    IndexSelected --;

                    if(IndexSelected == -1)
                    {
                        IndexSelected = Options.Count - 1;
                    }
                }
                else if(keyPressed == ConsoleKey.DownArrow)
                {
                    IndexSelected ++;

                    if(IndexSelected == Options.Count)
                    {
                        IndexSelected = 0;
                    }
                }
            } while (keyPressed != ConsoleKey.Enter);

            return IndexSelected;

        }
    }
    
}
