using Contoso.Entities;
using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Attachments.Abstractions;
using Regira.Entities.Web.Controllers.Abstractions;

namespace Contoso.API.Controllers;

// ApiControllerAttribute is added automatically by EntityControllerBase<TEntity>

// Student
[Route("students")]
public class StudentController : EntityControllerBase<Student, StudentDto, StudentInputDto>;

// Instructor
[Route("instructors")]
public class InstructorController : EntityControllerBase<Instructor, InstructorDto, InstructorInputDto>;
[Route("instructors")]
public class InstructorAttachmentController : EntityAttachmentControllerBase<InstructorAttachment, InstructorAttachmentDto, InstructorAttachmentInputDto>;

// Course
[Route("courses")]
public class CourseController : EntityControllerBase<Course, int, CourseSearchObject, CourseSortBy, CourseIncludes, CourseDto, CourseInputDto>;
[Route("courses")]
public class CourseAttachmentController : EntityAttachmentControllerBase<CourseAttachment>;

// Department
[Route("departments")]
public class DepartmentController : EntityControllerBase<Department, Guid, SearchObject<Guid>, DepartmentDto, DepartmentInputDto>;
