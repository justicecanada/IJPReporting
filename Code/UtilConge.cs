using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace IJPReporting.DateTools
{
    public enum EnumFeteStatique { Noel, LendemainNoel, JourAn, StJeanBaptiste, Confederation, JourSouvenir };


    public class UtilConge
    {

        public static bool isConge(DateTime uneJournee, bool isQuebec)
        {
            uneJournee = uneJournee.Date;
            if (uneJournee.DayOfWeek == DayOfWeek.Saturday || uneJournee.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
            if (uneJournee == getCongeFeteStatique(uneJournee.Year, EnumFeteStatique.Noel)
                || uneJournee == getCongeFeteStatique(uneJournee.Year, EnumFeteStatique.JourAn)
                || uneJournee == getCongeFeteStatique(uneJournee.Year, EnumFeteStatique.LendemainNoel)
                || uneJournee == getCongeFeteStatique(uneJournee.Year, EnumFeteStatique.Confederation)
                || uneJournee == getCongeFeteStatique(uneJournee.Year, EnumFeteStatique.JourSouvenir))
            {
                return true;
            }
            if (uneJournee == getFeteProvinciale(uneJournee.Year, isQuebec))
            {
                return true;
            }
            if (uneJournee == getFeteReineDollarPatriotes(uneJournee.Year))
            {
                return true;
            }
            if (uneJournee == getFeteDuTravail(uneJournee.Year))
            {
                return true;
            }
            if (uneJournee == getActionDeGrace(uneJournee.Year))
            {
                return true;
            }
            if (uneJournee == getVendrediSaint(uneJournee.Year))
            {
                return true;
            }
            if (uneJournee == getLundiDePaques(uneJournee.Year))
            {
                return true;
            }



            return false;
        }

        public static DateTime getCongeFeteStatique(int annee, EnumFeteStatique fete)
        {
            DateTime laDate = new DateTime();
            if (fete == EnumFeteStatique.Noel)
            {
                laDate = new DateTime(annee, 12, 25);
            }
            if (fete == EnumFeteStatique.JourAn)
            {
                laDate = new DateTime(annee, 1, 1);
            }
            if (fete == EnumFeteStatique.StJeanBaptiste)
            {
                laDate = new DateTime(annee, 6, 24);
            }
            if (fete == EnumFeteStatique.Confederation)
            {
                laDate = new DateTime(annee, 7, 1);
            }
            if (fete == EnumFeteStatique.LendemainNoel)
            {
                laDate = getCongeFeteStatique(annee, EnumFeteStatique.Noel).AddDays(1);
            }
            if (fete == EnumFeteStatique.JourSouvenir)
            {
                laDate = new DateTime(annee, 11, 11);
            }

            if (laDate.DayOfWeek == DayOfWeek.Sunday)
            {
                laDate = laDate.AddDays(1);
            }
            if (laDate.DayOfWeek == DayOfWeek.Saturday)
            {
                laDate = laDate.AddDays(2);
            }


            return laDate;
        }

        public static DateTime getFeteProvinciale(int annee, bool isQuebec)
        {
            //si Québec, retourne la St-Jean-Baptiste
            if (isQuebec)
            {
                return getCongeFeteStatique(annee, EnumFeteStatique.StJeanBaptiste);
            }
            else
            {
                return getFeriePremierLundiAout(annee);
            }

        }


        public static DateTime getFeriePremierLundiAout(int annee)
        {
            DateTime laDate = new DateTime(annee, 8, 1);
            while (laDate.DayOfWeek != DayOfWeek.Monday)
            {
                laDate = laDate.AddDays(1);
            }
            return laDate;
        }

        public static DateTime getFeteReineDollarPatriotes(int annee)
        {
            DateTime laDate = new DateTime(annee, 5, 24);
            while (laDate.DayOfWeek != DayOfWeek.Monday)
            {
                laDate = laDate.AddDays(-1);
            }
            return laDate;
        }

        public static DateTime getFeteDuTravail(int annee)
        {
            DateTime laDate = new DateTime(annee, 9, 1);
            while (laDate.DayOfWeek != DayOfWeek.Monday)
            {
                laDate = laDate.AddDays(1);
            }
            return laDate;
        }

        public static DateTime getActionDeGrace(int annee)
        {
            DateTime laDate = new DateTime(annee, 10, 1);
            while (laDate.DayOfWeek != DayOfWeek.Monday)
            {
                laDate = laDate.AddDays(1);
            }
            laDate = laDate.AddDays(7);
            return laDate;
        }

        public static DateTime getVendrediSaint(int annee)
        {
            int Month = 3;

            int G = annee % 19 + 1;

            int C = annee / 100 + 1;

            int X = (3 * C) / 4 - 12;

            int Y = (8 * C + 5) / 25 - 5;

            int Z = (5 * annee) / 4 - X - 10;

            int E = (11 * G + 20 + Y - X) % 30;
            if (E == 24) { E++; }
            if ((E == 25) && (G > 11)) { E++; }

            int N = 44 - E;
            if (N < 21) { N = N + 30; }

            int P = (N + 7) - ((Z + N) % 7);

            if (P > 31)
            {
                P = P - 31;
                Month = 4;
            }
            return new DateTime(annee, Month, P).AddDays(-2);
        }

        public static DateTime getLundiDePaques(int annee)
        {
            DateTime laDate = getVendrediSaint(annee);
            return laDate.AddDays(3);
        }

        public static List<DateTime> getJoursFeries(int annee, bool isQuebec)
        {
            List<DateTime> listeFeries = new List<DateTime>();
            listeFeries.Add(getCongeFeteStatique(annee, EnumFeteStatique.Confederation));
            listeFeries.Add(getCongeFeteStatique(annee, EnumFeteStatique.JourAn));
            listeFeries.Add(getCongeFeteStatique(annee, EnumFeteStatique.JourSouvenir));
            listeFeries.Add(getCongeFeteStatique(annee, EnumFeteStatique.LendemainNoel));
            listeFeries.Add(getCongeFeteStatique(annee, EnumFeteStatique.Noel));
            listeFeries.Add(getFeteReineDollarPatriotes(annee));
            listeFeries.Add(getFeteDuTravail(annee));
            listeFeries.Add(getActionDeGrace(annee));
            listeFeries.Add(getVendrediSaint(annee));
            listeFeries.Add(getLundiDePaques(annee));
            listeFeries.Add(getFeteProvinciale(annee, isQuebec));

            return listeFeries;
        }
    }


    public static class CongeExtensions
    {

        public static bool isConge(this DateTime date, bool isQuebec)
        {
            return UtilConge.isConge(date, isQuebec);
        }


        public static DateTime getToNJoursOuvrablesApres(this DateTime date, int nbJoursIntervalle, bool isQuebec)
        {
            int nbJours = 0;
            DateTime curDate = date;
            while (nbJours < nbJoursIntervalle)
            {
                curDate = curDate.AddDays(1);
                if (!curDate.isConge(isQuebec))
                {
                    nbJours++;
                }
            }
            return curDate;
        }

        public static DateTime getToNJoursOuvrablesAvant(this DateTime date, int nbJoursIntervalle, bool isQuebec)
        {
            int nbJours = 0;
            DateTime curDate = date;
            while (nbJours < nbJoursIntervalle)
            {
                curDate = curDate.AddDays(-1);
                if (!curDate.isConge(isQuebec))
                {
                    nbJours++;
                }
            }
            return curDate;
        }

    }
}




