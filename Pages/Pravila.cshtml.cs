using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class PravilaModel : PageModel
{
    public string MainTitle { get; set; }

    public List<Paragraph> Paragraphs { get; set; }

    public void OnGet()
    {
        // Initialize your data here
        MainTitle = "Pravila lige";
        Paragraphs = new List<Paragraph>
        {
            new Paragraph
            {
                Subtitle = "Lige",
                Content = "Igrači se nalaze u ligama od pet igrača (ukoliko ima dovoljno prijavljenih igrača) i igraju jednokružno, svak sa svakim."
            },
            new Paragraph
            {
                Subtitle = "Dogovaranje susreta",
                Content = "Igrači sami dogovaraju lokaciju i vrijeme odigravanja susreta, te sami plaćaju termin i dogovaraju se oko loptica."
            },
            new Paragraph
            {
                Subtitle = "Odigravanje susreta",
                Content = "Igraju se dva seta, a pri rezultatu 1-1 u setovima, igra se tie-break do 10. Ako su oba igrača suglasna, može se odigrati i redovni treći set i prijavljuje se takav rezultat."
            },            
            new Paragraph
            {
                Subtitle = "Trajanje lige",
                Content = "Liga u pravilu traje mjesec dana i u tom periodu potrebno je odigrati sve mečeve. Iznimno produžavanje trajanja lige moguće je dogovorom igrača zbog npr. loših vremenskih uvjeta."
            },
            new Paragraph
            {
                Subtitle = "Bodovanje",
                Content = "Za odigrani susret dobija se 1 bod, za odigrani susret i pobjedu 3 boda."
            },
            new Paragraph
            {
                Subtitle = "Neodigrani susreti",
                Content = "Ukoliko je igrač pokušao dogovorit susret i ponudio više opcija, a protivnik nijednu, ima pravo tražit bodove za taj susret (Walk over - WO). Pobjednik dobiva 3 boda, poraženi 0, pobjedniku se meč broji u odigrane, poraženom ne. Ni jednom ni drugom igraču susret ne utječe na ELO"
            },
            new Paragraph
            {
                Subtitle = "Poredak",
                Content = " Poredak igrača u ligi ovisi o sljedećem i to redom: broju bodova, broju odigranih mečeva, međusobnom skoru, omjeru odigranih/osvojenih setova, omjeru odigranih/osvojenih gemova, ELO rejtingu."
            },
            new Paragraph
            {
                Subtitle = "Napredovanje i ispadanje iz lige",
                Content = " Pobjednik lige napreduje u ligu više. Posljednji (ili posljednja dvojica, ovisno o ligi) ispadaju u nižu ligu."
            },
            new Paragraph
            {
                Subtitle = "Novi igrači",
                Content = "Odbor igrača odlučuje o svrstavanju u ligu novog igrača."
            },
            new Paragraph
            {
                Subtitle = "Nastupanje na vlastitu odgovornost",
                Content = "Igrači nastupaju na vlastitu odgovornost te organizator ne snosi odgovornost za eventualne nastale ozljede ili narušeno zdravlje."
            },
            new Paragraph
            {
                Subtitle = "GDPR",
                Content = "Prijavom u ligu dajete i privolu za korištenjem vaših osobnih podataka."
            },
            new Paragraph
            {
                Subtitle = "Stil igre",
                Content = "Svaki igrač u ligi ima pravo igrati svojim stilom dokle god je to po pravilima teniske igre."
            }
        };
    }
}

public class Paragraph
{
    public string Subtitle { get; set; }
    public string Content { get; set; }
}
