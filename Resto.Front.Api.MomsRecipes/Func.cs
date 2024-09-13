﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JetBrainsNotNull = JetBrains.Annotations.NotNullAttribute;
using SystemNotNull = System.Diagnostics.CodeAnalysis.NotNullAttribute;

using JetBrains.Annotations; // Для JetBrains.Annotations.NotNullAttribute
using System.Diagnostics.CodeAnalysis; // Для System.Diagnostics.CodeAnalysis.NotNullAttribute


namespace Resto.Front.Api.MomsRecipes
{
    public static class Func
    {
        public static DateTime ConvertDateTime(string Date)
        {
            DateTime date = new DateTime();
            try
            {
                string CurrentPattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
                string[] Split = new string[] { "-", "/", @"\", "." };
                string[] Patternvalue = CurrentPattern.Split(Split, StringSplitOptions.None);
                string[] DateSplit = Date.Split(Split, StringSplitOptions.None);
                string NewDate = "";
                if (Patternvalue[0].ToLower().Contains("d") == true && Patternvalue[1].ToLower().Contains("m") == true && Patternvalue[2].ToLower().Contains("y") == true)
                {
                    NewDate = DateSplit[1] + "/" + DateSplit[0] + "/" + DateSplit[2];
                }
                else if (Patternvalue[0].ToLower().Contains("m") == true && Patternvalue[1].ToLower().Contains("d") == true && Patternvalue[2].ToLower().Contains("y") == true)
                {
                    NewDate = DateSplit[0] + "/" + DateSplit[1] + "/" + DateSplit[2];
                }
                else if (Patternvalue[0].ToLower().Contains("y") == true && Patternvalue[1].ToLower().Contains("m") == true && Patternvalue[2].ToLower().Contains("d") == true)
                {
                    NewDate = DateSplit[2] + "/" + DateSplit[0] + "/" + DateSplit[1];
                }
                else if (Patternvalue[0].ToLower().Contains("y") == true && Patternvalue[1].ToLower().Contains("d") == true && Patternvalue[2].ToLower().Contains("m") == true)
                {
                    NewDate = DateSplit[2] + "/" + DateSplit[1] + "/" + DateSplit[0];
                }
                date = DateTime.Parse(NewDate, Thread.CurrentThread.CurrentCulture);
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

            return date;

        }
    }
}
