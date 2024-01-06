namespace MatoProductivity.ViewModels
{
    public class FinishedChooiseEvenArgs
    {

        public FinishedChooiseEvenArgs(string address , Core.Location.Location location)
        {
            Address=address;
            Location=location;
        }
        private string _address;

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
            }
        }


        private Core.Location.Location _location;

        public Core.Location.Location Location
        {
            get { return _location; }
            set
            {
                _location = value;
            }
        }
    }
}