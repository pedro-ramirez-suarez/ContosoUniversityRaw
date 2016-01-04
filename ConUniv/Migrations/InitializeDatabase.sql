/****** Object:  Table [dbo].[Course]    Script Date: 12/4/2015 10:44:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Credits] [int] NOT NULL,
	[DepartmentID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[CourseInstructor]    Script Date: 12/4/2015 10:44:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseInstructor](
	[Id] [uniqueidentifier] NOT NULL,
	[CourseID] [uniqueidentifier] NOT NULL,
	[InstructorID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CourseInstructor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[Department]    Script Date: 12/4/2015 10:44:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Budget] [money] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[InstructorID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[Enrollment]    Script Date: 12/4/2015 10:44:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollment](
	[Id] [uniqueidentifier] NOT NULL,
	[CourseID] [uniqueidentifier] NOT NULL,
	[StudentID] [uniqueidentifier] NOT NULL,
	[Grade] [int] NULL,
 CONSTRAINT [PK_Enrollment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[OfficeAssignment]    Script Date: 12/4/2015 10:44:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OfficeAssignment](
	[Id] [uniqueidentifier] NOT NULL,
	[InstructorID] [uniqueidentifier] NOT NULL,
	[Location] [nvarchar](50) NULL,
 CONSTRAINT [PK_OfficeAssignment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[Person]    Script Date: 12/4/2015 10:44:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[Id] [uniqueidentifier] NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[HireDate] [datetime] NULL,
	[EnrollmentDate] [datetime] NULL,
	[Discriminator] [nvarchar](128) NOT NULL CONSTRAINT [DF__Person__Discrimi__108B795B]  DEFAULT ('Instructor'),
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  View [dbo].[Instructor]    Script Date: 12/4/2015 10:44:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Instructor]
AS
SELECT        Id, LastName, FirstName, HireDate, Discriminator
FROM            dbo.Person
WHERE Discriminator = 'Instructor'

GO
/****** Object:  View [dbo].[Student]    Script Date: 12/4/2015 10:44:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Student]
AS
SELECT        Id, LastName, FirstName, EnrollmentDate, Discriminator
FROM            dbo.Person
WHERE Discriminator = 'Student'

GO
ALTER TABLE [dbo].[CourseInstructor]  WITH CHECK ADD  CONSTRAINT [FK_CourseInstructor_Course] FOREIGN KEY([CourseID])
REFERENCES [dbo].[Course] ([Id])
GO
ALTER TABLE [dbo].[CourseInstructor] CHECK CONSTRAINT [FK_CourseInstructor_Course]
GO
ALTER TABLE [dbo].[CourseInstructor]  WITH CHECK ADD  CONSTRAINT [FK_CourseInstructor_Person] FOREIGN KEY([InstructorID])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[CourseInstructor] CHECK CONSTRAINT [FK_CourseInstructor_Person]
GO
ALTER TABLE [dbo].[Department]  WITH CHECK ADD  CONSTRAINT [FK_Department_Person] FOREIGN KEY([InstructorID])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[Department] CHECK CONSTRAINT [FK_Department_Person]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_Course] FOREIGN KEY([CourseID])
REFERENCES [dbo].[Course] ([Id])
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK_Enrollment_Course]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_Person] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK_Enrollment_Person]
GO
ALTER TABLE [dbo].[OfficeAssignment]  WITH CHECK ADD  CONSTRAINT [FK_OfficeAssignment_Person] FOREIGN KEY([InstructorID])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[OfficeAssignment] CHECK CONSTRAINT [FK_OfficeAssignment_Person]
GO


--Initial data
INSERT [dbo].[Person] ([Id], [LastName], [FirstName], [HireDate], [EnrollmentDate], [Discriminator]) VALUES (N'5e9d9678-2556-48a4-9136-0abfe86977ec', N'Babbage', N'Charles', CAST(N'1970-01-02 00:00:00.000' AS DateTime), NULL, N'Instructor')
GO
INSERT [dbo].[Person] ([Id], [LastName], [FirstName], [HireDate], [EnrollmentDate], [Discriminator]) VALUES (N'f12bd457-f718-40d9-b920-20c1eabe9a89', N'Touring', N'Alan', CAST(N'2015-08-04 11:35:30.000' AS DateTime), CAST(N'2015-08-25 11:35:30.673' AS DateTime), N'Instructor')
GO
INSERT [dbo].[Person] ([Id], [LastName], [FirstName], [HireDate], [EnrollmentDate], [Discriminator]) VALUES (N'66394f0a-ff28-437b-afbc-22dc4d7ffc80', N'Albert', N'Einstein', CAST(N'1970-01-01 00:00:00.003' AS DateTime), NULL, N'Instructor')
GO
INSERT [dbo].[Person] ([Id], [LastName], [FirstName], [HireDate], [EnrollmentDate], [Discriminator]) VALUES (N'6d63eeb9-ecef-41a9-b869-272bb4315f00', N'Marie', N'Curie', CAST(N'1970-01-01 00:00:00.003' AS DateTime), NULL, N'Instructor')
GO
INSERT [dbo].[Person] ([Id], [LastName], [FirstName], [HireDate], [EnrollmentDate], [Discriminator]) VALUES (N'6144993a-c7c5-4c5d-b99f-2d5d8ca57884', N'Ramirez Suarez', N'Pedro', NULL, CAST(N'2014-12-31 06:00:00.000' AS DateTime), N'Student')
GO
INSERT [dbo].[Person] ([Id], [LastName], [FirstName], [HireDate], [EnrollmentDate], [Discriminator]) VALUES (N'9c93e69f-76e2-4989-8c4c-49a73cc7c09f', N'Doe', N'John', NULL, CAST(N'2015-12-20 16:23:46.000' AS DateTime), N'Student')
GO
INSERT [dbo].[Person] ([Id], [LastName], [FirstName], [HireDate], [EnrollmentDate], [Discriminator]) VALUES (N'ab20b675-44dd-4ea1-a6c0-564eb548bed4', N'Newton', N'Isaac', CAST(N'2016-01-02 13:08:38.000' AS DateTime), NULL, N'Instructor')
GO
INSERT [dbo].[Department] ([Id], [Name], [Budget], [StartDate], [InstructorID]) VALUES (N'f258a96c-cfcc-4413-8dca-19561e504c0f', N'Physics', CAST(1000000.00 AS Decimal(18, 2)), CAST(N'2015-12-04 00:00:00.000' AS DateTime), N'66394f0a-ff28-437b-afbc-22dc4d7ffc80')
GO
INSERT [dbo].[Department] ([Id], [Name], [Budget], [StartDate], [InstructorID]) VALUES (N'1c2d7154-3f2c-4720-b04c-1ea5c7dbc57d', N'Mathematics', CAST(10000000.00 AS Decimal(18, 2)), CAST(N'1969-12-31 18:00:00.017' AS DateTime), N'ab20b675-44dd-4ea1-a6c0-564eb548bed4')
GO
INSERT [dbo].[Department] ([Id], [Name], [Budget], [StartDate], [InstructorID]) VALUES (N'39bcdf72-0d6b-45db-8603-5e545f545d27', N'Computer Science', CAST(1000000.00 AS Decimal(18, 2)), CAST(N'1969-12-31 18:00:00.070' AS DateTime), N'f12bd457-f718-40d9-b920-20c1eabe9a89')
GO
INSERT [dbo].[Department] ([Id], [Name], [Budget], [StartDate], [InstructorID]) VALUES (N'd8a7a3bb-50cf-4b82-9a5d-8149c58bfd10', N'Chemistry', CAST(10000000.00 AS Decimal(18, 2)), CAST(N'2015-12-14 00:00:00.000' AS DateTime), N'6d63eeb9-ecef-41a9-b869-272bb4315f00')
GO
INSERT [dbo].[Course] ([Id], [Title], [Credits], [DepartmentID]) VALUES (N'b5548778-e3d3-4bba-8358-0076ab538536', N'Quantum Mechanics I', 5, N'f258a96c-cfcc-4413-8dca-19561e504c0f')
GO
INSERT [dbo].[Course] ([Id], [Title], [Credits], [DepartmentID]) VALUES (N'2533fc2d-ee99-4a17-9a03-0927e7d9a837', N'Chemistry 1', 10, N'd8a7a3bb-50cf-4b82-9a5d-8149c58bfd10')
GO
INSERT [dbo].[Course] ([Id], [Title], [Credits], [DepartmentID]) VALUES (N'7f1541b9-4534-42d3-b45e-103e1f68f0cc', N'Calculos 101', 10, N'1c2d7154-3f2c-4720-b04c-1ea5c7dbc57d')
GO
INSERT [dbo].[Course] ([Id], [Title], [Credits], [DepartmentID]) VALUES (N'14a5c9fc-61a6-43ff-b9e7-21bd23f9589e', N'Data bases', 100, N'39bcdf72-0d6b-45db-8603-5e545f545d27')
GO
INSERT [dbo].[Course] ([Id], [Title], [Credits], [DepartmentID]) VALUES (N'f89d6922-eefe-461e-926f-31bdf9dfda4f', N'Physics 1', 10, N'f258a96c-cfcc-4413-8dca-19561e504c0f')
GO
INSERT [dbo].[Course] ([Id], [Title], [Credits], [DepartmentID]) VALUES (N'0cdeb6ed-419f-4094-805b-5fae5ec887eb', N'Programming', 10, N'39bcdf72-0d6b-45db-8603-5e545f545d27')
GO
INSERT [dbo].[Course] ([Id], [Title], [Credits], [DepartmentID]) VALUES (N'33a9456c-f678-4303-8f86-7a384d676f8e', N'Distributed Systems', 5, N'39bcdf72-0d6b-45db-8603-5e545f545d27')
GO
INSERT [dbo].[Course] ([Id], [Title], [Credits], [DepartmentID]) VALUES (N'2e013a00-220c-4802-bb62-c0e68fbc0de4', N'Chemistry Advanced', 10, N'd8a7a3bb-50cf-4b82-9a5d-8149c58bfd10')
GO
INSERT [dbo].[Course] ([Id], [Title], [Credits], [DepartmentID]) VALUES (N'8e1e0b16-c3d4-4cd7-9233-ed5f9e5a497c', N'Operating Systems', 10, N'39bcdf72-0d6b-45db-8603-5e545f545d27')
GO
INSERT [dbo].[CourseInstructor] ([Id], [CourseID], [InstructorID]) VALUES (N'b1b1636a-92f4-4a1b-b393-1c86fc10bd6e', N'2e013a00-220c-4802-bb62-c0e68fbc0de4', N'6d63eeb9-ecef-41a9-b869-272bb4315f00')
GO
INSERT [dbo].[CourseInstructor] ([Id], [CourseID], [InstructorID]) VALUES (N'46ce4920-b638-469a-9265-218811db9370', N'8e1e0b16-c3d4-4cd7-9233-ed5f9e5a497c', N'5e9d9678-2556-48a4-9136-0abfe86977ec')
GO
INSERT [dbo].[CourseInstructor] ([Id], [CourseID], [InstructorID]) VALUES (N'94045336-8b05-4090-8694-3864756375ac', N'2533fc2d-ee99-4a17-9a03-0927e7d9a837', N'6d63eeb9-ecef-41a9-b869-272bb4315f00')
GO
INSERT [dbo].[CourseInstructor] ([Id], [CourseID], [InstructorID]) VALUES (N'cddc18cf-c973-4994-a725-870f5ce222d3', N'14a5c9fc-61a6-43ff-b9e7-21bd23f9589e', N'5e9d9678-2556-48a4-9136-0abfe86977ec')
GO
INSERT [dbo].[CourseInstructor] ([Id], [CourseID], [InstructorID]) VALUES (N'8c928503-6c20-4fea-b753-d06d004f62af', N'0cdeb6ed-419f-4094-805b-5fae5ec887eb', N'f12bd457-f718-40d9-b920-20c1eabe9a89')
GO
INSERT [dbo].[CourseInstructor] ([Id], [CourseID], [InstructorID]) VALUES (N'474a0d73-071e-4f07-9f63-d57ceaab22b9', N'b5548778-e3d3-4bba-8358-0076ab538536', N'66394f0a-ff28-437b-afbc-22dc4d7ffc80')
GO
INSERT [dbo].[Enrollment] ([Id], [CourseID], [StudentID], [Grade]) VALUES (N'770a6694-4edb-4ef7-b642-1c2eb99ed437', N'0cdeb6ed-419f-4094-805b-5fae5ec887eb', N'9c93e69f-76e2-4989-8c4c-49a73cc7c09f', NULL)
GO
INSERT [dbo].[Enrollment] ([Id], [CourseID], [StudentID], [Grade]) VALUES (N'e7bccad2-5577-4111-9693-a6bce4018bf2', N'0cdeb6ed-419f-4094-805b-5fae5ec887eb', N'6144993a-c7c5-4c5d-b99f-2d5d8ca57884', 1)
GO
INSERT [dbo].[Enrollment] ([Id], [CourseID], [StudentID], [Grade]) VALUES (N'4ad32471-6868-4a97-9965-af6b4c865b62', N'b5548778-e3d3-4bba-8358-0076ab538536', N'6144993a-c7c5-4c5d-b99f-2d5d8ca57884', NULL)
GO
INSERT [dbo].[OfficeAssignment] ([Id], [InstructorID], [Location]) VALUES (N'fc29ab6e-5979-434a-9d43-1029cab9eb3e', N'ab20b675-44dd-4ea1-a6c0-564eb548bed4', NULL)
GO
INSERT [dbo].[OfficeAssignment] ([Id], [InstructorID], [Location]) VALUES (N'32dcdb68-5ca7-46e8-a47b-214b71970f69', N'66394f0a-ff28-437b-afbc-22dc4d7ffc80', NULL)
GO
INSERT [dbo].[OfficeAssignment] ([Id], [InstructorID], [Location]) VALUES (N'670c5651-ef0f-4ba1-88d3-4214d66ada93', N'f12bd457-f718-40d9-b920-20c1eabe9a89', NULL)
GO
INSERT [dbo].[OfficeAssignment] ([Id], [InstructorID], [Location]) VALUES (N'b4f9c1fa-066d-4901-90ed-e844fc2b0bfc', N'5e9d9678-2556-48a4-9136-0abfe86977ec', NULL)
GO
INSERT [dbo].[Migration] ([Id], [Script], [ExecutedOn]) VALUES (N'0fbd84db-c7cb-4163-bcb0-de7436663bb1', N'initializedatabase.sql', CAST(N'2015-12-04 10:59:49.937' AS DateTime))
GO
