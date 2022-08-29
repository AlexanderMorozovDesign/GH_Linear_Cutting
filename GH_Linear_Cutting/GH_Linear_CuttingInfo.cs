using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace GH_Linear_Cutting
{
    public class GH_Linear_CuttingInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "GHLinearCutting";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("719eac61-0a98-47c9-b054-a05d79644d33");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Alexander Morozov";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "https://github.com/AlexanderMorozovDesign";
            }
        }
    }
}
