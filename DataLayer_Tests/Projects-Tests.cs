﻿using NUnit.Framework;
using Rsff.DataLayer;
using System;
using System.Data;

namespace DataLayer_Tests
{
    [TestFixture]
    public class DaoTests
    {
        #region TestProjectInsert
        [Test]
        public void TestProjectInsert()
        {
            //fake up data
            Random random = new Random();

            string address = string.Format("Address-{0}", Guid.NewGuid().ToString());
            string APN = String.Format("{0}-{1}-{2}", random.Next(0, 999), random.Next(0, 999), random.Next(0, 99));
            string notes = string.Format("Random notes for project id # {0}.", Guid.NewGuid().ToString());
            int planCheckNumber = random.Next(1, 9999);
            string projectName = string.Format("Project Name {0}", Guid.NewGuid().ToString());

            //insert into db
            DaoProjects dao = new DaoProjects();
            int rowID = dao.InsertProject(address, APN, notes, planCheckNumber, projectName);

            //verify row inserted
            Assert.Greater(rowID, 0);

            //get data back from db and compare to verify no mapping errors
            DataRow row = dao.GetProjectByProjectID(rowID);

            Assert.AreEqual(rowID, Convert.ToInt32(row["ProjectID"]));
            Assert.AreEqual(address, Convert.ToString(row["Address"]));
            Assert.AreEqual(APN, Convert.ToString(row["APN"]));
            Assert.AreEqual(notes, Convert.ToString(row["Notes"]));
            Assert.AreEqual(planCheckNumber, Convert.ToInt32(row["PlanCheckNumber"]));
            Assert.AreEqual(projectName, Convert.ToString(row["ProjectName"]));

        } 
        #endregion

        #region TestGetProjectCount
        [Test(Description = "Tests Count Functionality")]
        public void TestGetProjectCount()
        {
            DaoProjects dao = new DaoProjects();
            int rowCount = dao.GetProjectsCount();
            Assert.Greater(rowCount, 0);

        } 
        #endregion

        #region TestGetProjectPage
        [Test(Description = "Tests Paging Logic")]
        public void TestGetProjectPage()
        {
            const int ROW_FROM = 5;
            const int ROW_TO = 14;

            DaoProjects dao = new DaoProjects();
            DataSet ds = dao.GetProjectPage(ROW_FROM, ROW_TO);
            Assert.AreEqual(2, ds.Tables.Count);
            //verify 1st table has the correct number of rows of data
            Assert.AreEqual(ROW_TO - ROW_FROM + 1, ds.Tables[0].Rows.Count);

            //verify 2nd table has correct row count
            int rowCount = dao.GetProjectsCount();
            Assert.Greater(rowCount, 0);
            Assert.AreEqual(rowCount, Convert.ToInt32(ds.Tables[1].Rows[0][0]));
        } 
        #endregion

        #region TestProjectUpdate
        [Test]
        public void TestProjectUpdate()
        {
            //fake up data
            Random random = new Random();

            string projectAddress = string.Format("Address-{0}", Guid.NewGuid().ToString());
            string APN = String.Format("{0}-{1}-{2}", random.Next(0, 999), random.Next(0, 999), random.Next(0, 99));
            string Notes = string.Format("Random notes for project id # {0}.", Guid.NewGuid().ToString());
            int planCheckNumber = random.Next(1, 9999);
            string projectName = string.Format("Project Name {0}", Guid.NewGuid().ToString());

            //insert into db
            DaoProjects dao = new DaoProjects();
            int projectID = dao.InsertProject(projectAddress, APN, Notes, planCheckNumber, projectName);

            //verify something came back
            Assert.Greater(projectID,0);

            //get project back from db
            DataRow row = dao.GetProjectByProjectID(projectID);
            //verify something came back
            Assert.IsNotNull(row);

            //verify no mapping errors
            Assert.AreEqual(projectID, Convert.ToInt32(row["ProjectID"]));
            Assert.AreEqual(projectAddress, Convert.ToString(row["Address"]));
            Assert.AreEqual(APN, Convert.ToString(row["APN"]));
            Assert.AreEqual(Notes, Convert.ToString(row["Notes"]));
            Assert.AreEqual(planCheckNumber, Convert.ToInt32(row["PlanCheckNumber"]));
            Assert.AreEqual(projectName, Convert.ToString(row["ProjectName"]));

            //Update that row with random values
            projectAddress = String.Format("Random Project Address - {0}", Guid.NewGuid().ToString());
            APN = String.Format("Random APN - {0}", Guid.NewGuid().ToString());
            Notes = String.Format("Random Notes - {0}", Guid.NewGuid().ToString());
            planCheckNumber = random.Next();
            projectName = String.Format("Random Project Name - {0}", Guid.NewGuid().ToString());
            
            int rowsAffected = dao.UpdateProject(projectID, projectAddress, APN, Notes, planCheckNumber, projectName);
            //verify 1 and only 1 row was updated
            Assert.AreEqual(1, rowsAffected);

            //get project back from db
            row = dao.GetProjectByProjectID(projectID);
            //verify something came back
            Assert.IsNotNull(row);

            //verify no mapping errors
            Assert.AreEqual(projectID, Convert.ToInt32(row["ProjectID"]));
            Assert.AreEqual(projectAddress, Convert.ToString(row["Address"]));
            Assert.AreEqual(APN, Convert.ToString(row["APN"]));
            Assert.AreEqual(Notes, Convert.ToString(row["Notes"]));
            Assert.AreEqual(planCheckNumber, Convert.ToInt32(row["PlanCheckNumber"]));
            Assert.AreEqual(projectName, Convert.ToString(row["ProjectName"]));


        } 
        #endregion

        #region TestGetProjectByProjectID
        [Test]
        public void TestGetProjectByProjectID()
        {
            //fake up data
            Random random = new Random();

            string projectAddress = string.Format("Address-{0}", Guid.NewGuid().ToString());
            string APN = String.Format("{0}-{1}-{2}", random.Next(0, 999), random.Next(0, 999), random.Next(0, 99));
            string Notes = string.Format("Random notes for project id # {0}.", Guid.NewGuid().ToString());
            int planCheckNumber = random.Next(1, 9999);
            string projectName = string.Format("Project Name {0}", Guid.NewGuid().ToString());

            //insert into db
            DaoProjects dao = new DaoProjects();
            int projectID = dao.InsertProject(projectAddress, APN, Notes, planCheckNumber, projectName);

            //verify something came back
            Assert.Greater(projectID, 0);

            //get that project
            DataRow row = dao.GetProjectByProjectID(projectID);

            //verify something came back
            Assert.IsNotNull(row);

            //assert the row id is the same as the project id
            Assert.AreEqual(projectID, Convert.ToInt32(row["ProjectID"]));
        } 
        #endregion

        #region TestProjectSoftDelete
        [Test]
        public void TestProjectSoftDelete()
        {
            //fake up data
            Random random = new Random();

            string address = string.Format("Address-{0}", Guid.NewGuid().ToString());
            string APN = String.Format("{0}-{1}-{2}", random.Next(0, 999), random.Next(0, 999), random.Next(0, 99));
            string notes = string.Format("Random notes for project id # {0}.", Guid.NewGuid().ToString());
            int planCheckNumber = random.Next(1, 9999);
            string projectName = string.Format("Project Name {0}", Guid.NewGuid().ToString());

            //insert into db
            DaoProjects dao = new DaoProjects();
            int projectID = dao.InsertProject(address, APN, notes, planCheckNumber, projectName);

            //verify row id was created
            Assert.Greater(projectID, 0);

            //get it back
            DataRow row = dao.GetProjectByProjectID(projectID);
            Assert.IsNotNull(row);

            //soft delete it
            int rowsAffected = dao.SoftDeleteProject(projectID);
            //verify 1 and only 1 row deleted
            Assert.AreEqual(1, rowsAffected);

            //try and get it back
            try
            {
                row = dao.GetProjectByProjectID(projectID);
            }
            catch (System.IndexOutOfRangeException)
            {
                ; //this is what sql burps up
            }

        } 
        #endregion



        [Test]
        public void TestSearchProjectByAddress()
        {
            Assert.Fail();
        }

        [Test]
        public void TestSearchProjectByAPN()
        {
            Assert.Fail();
        }

        [Test]
        public void TestSearchProjectByNotes()
        {
            Assert.Fail();
        }

        [Test]
        public void TestSearchProjectByPlanCheckNumber()
        {
            Assert.Fail();
        }

        [Test]
        public void TestSearchProjectByProjectName()
        {
            Assert.Fail();
        }


    }
}
