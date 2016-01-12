using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchsimLib
{
    public class HelperFunctions
    {


        public static string RemoveSpecialCharacters(string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ' || c == '_' || c == '-' || c == '(' || c == ')' || c == '/' || c == '.')
                {
                    sb.Append(c);
                }
            }
           
            string newString = sb.ToString(); 

            

            if (newString.Length > 50) return newString.Substring(0, 50);
            else return newString;
        }



        public static string RemoveInvalidCharsInNumericFields(string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || c == '.')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
