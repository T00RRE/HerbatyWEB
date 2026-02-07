namespace Firma.Data.Data
{
    public class TowarTag
    {
        public int IdTowaru { get; set; }
        public virtual Towar Towar { get; set; }

        public int IdTagu { get; set; }
        public virtual Tag Tag { get; set; }
    }
}