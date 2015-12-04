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
    class StudentController_spec: nspec
    {
        StudentController controller;

        //This method runs before any test
        void before_each()
        {   
            //Add any initialization code here
            //Uncomment this line to debug the tests
            //System.Diagnostics.Debugger.Launch();
            controller = new StudentController(); 
        }
            
        //Simple test for the Index method
        void specify_controllerIndex()
        { 
            var result = controller.Index() as ViewResult;
            string StudentList = result.ViewBag.Students as string;
            StudentList.should_not_be_empty(); 
        }

        //Add Create, Edit and Delete tests in here
        //Check documentation here:  http://nspec.org/#helloworld

    }
}
