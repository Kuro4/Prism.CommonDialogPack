namespace Prism.CommonDialogPack.Events
{
    public class ProgressEventValue
    {
        public string Text { get; set; }
        public double Value { get; set; }

        public ProgressEventValue()
        {

        }

        public ProgressEventValue(double value)
        {
            this.Value = value;
        }
    }
}
