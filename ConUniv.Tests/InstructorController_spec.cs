using ConUniv.Controllers;
using NSpec;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ConUniv.Tests
{
    class InstructorController_spec: nspec
    {
        InstructorController controller;

        //This method runs before any test
        void before_each()
        {   
            //Add any initialization code here
            //Uncomment this line to debug the tests
            //System.Diagnostics.Debugger.Launch();
            controller = new InstructorController(); 
        }
            
        //Simple test for the Index method
        void specify_controllerIndex()
        { 
            var result = controller.Index() as ViewResult;
            string InstructorList = result.ViewBag.Instructors as string;
            InstructorList.should_not_be_empty(); 
        }

        //Add Create, Edit and Delete tests in here
        //Check documentation here:  http://nspec.org/#helloworld

    }
}
