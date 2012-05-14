namespace RezNetUsage.Core {
    using System;

    public partial class Chambres {

        public double MaximumSum
        {
            get
            {
                return (double)this.Tables[0].Compute("Sum(Maximum)", String.Empty);
            }
        }

    }
}
