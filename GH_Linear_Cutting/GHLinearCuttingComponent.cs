using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace GH_Linear_Cutting
{
    public class GHLinearCuttingComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GHLinearCuttingComponent(): base("1D Liner Cutting", "1DLinCut", "1D Liner Cutting", "1D Liner Cutting", "Fabrication")
        {
            this.Message = "1D Liner Cutting";
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Desired Cuts", "Cuts", "Lengths of desired cuts", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Amount Stock", "Amount", "Amount of blanks for each length", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Whip Length", "Whip Length", "Whip Length", GH_ParamAccess.item, 6000);
            pManager.AddIntegerParameter("End Cut", "End Cut", "Cut ends of the workpiece beam", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("Tool Width", "Tool Width", "Tool Width", GH_ParamAccess.item, 5);
            pManager.AddIntegerParameter("Headless Retreat", "Headless Retreat", "Headless Retreat", GH_ParamAccess.item, 100);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("StoLen", "StLen", "Stock Length", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("AmoWhi", "AmountWhips", "Amount of whips", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Waste", "Waste", "Waste", GH_ParamAccess.list);
            pManager.AddIntegerParameter("UsingLen", "UsingLen", "Using Length", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<int> desiredLengths = new List<int>();  // cutting length 
            List<int> amount = new List<int>();  // quantity for each position for cutting
            int whipLength = 6000; // дcutting length
            int endSawCut = 10; // end cut
            int toolWidth = 5;  // tool saw width
            int headlessRetreat = 100;  // бunconditional withdrawal

            // бdata entry block in the API interface of my program
            if (!DA.GetDataList(0, desiredLengths)) return;
            if (!DA.GetDataList(1, amount)) return;
            if (!DA.GetData(2, ref whipLength)) return;
            if (!DA.GetData(3, ref endSawCut)) return;
            if (!DA.GetData(4, ref toolWidth)) return;
            if (!DA.GetData(5, ref headlessRetreat)) return;


            // class for nesting calculation
            LinerCuttingClass linerCutting = new LinerCuttingClass(desiredLengths, amount, whipLength, endSawCut, toolWidth, headlessRetreat);

            List<List<int>> cuts = linerCutting.GetCuts();

            GH_Structure<GH_Integer> Tree = new GH_Structure<GH_Integer>(); // list of lists with cutting for each whip

            for (int i = 0; i < cuts.Count; i++)
            {
                Tree.AppendRange(cuts[i].Select(j => new GH_Integer(j)), new GH_Path(new int[] { 0, i }));
            }

            List<int> repeats = linerCutting.GetRepeats();   //list of the number of repetitions of the cutting mask per whip
            List<int> retreat = linerCutting.GetRetreats();  // waste list for each whip
            List<int> usingLength = linerCutting.GetUsingLength();  // list of used lengths for cutting for each workpiece

            // the output block of the received data in the API interface of my program
            DA.SetDataTree(0, Tree);  // list of lists with cutting for each whip
            DA.SetDataList(1, repeats);  //list of the number of repetitions of the cutting mask on the whip
            DA.SetDataList(2, retreat);  // waste list for each whip
            DA.SetDataList(3, usingLength); // list of used lengths for each piece
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2a701e09-59ff-4aee-8f36-c1c7620d7497"); }
        }
    }
}
