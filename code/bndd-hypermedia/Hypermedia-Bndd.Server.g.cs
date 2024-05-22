#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using RESTyard.AspNetCore.Hypermedia;
using RESTyard.AspNetCore.Hypermedia.Actions;
using RESTyard.AspNetCore.Hypermedia.Attributes;
using RESTyard.AspNetCore.Hypermedia.Extensions;
using RESTyard.AspNetCore.Hypermedia.Links;
using RESTyard.AspNetCore.Query;
using RESTyard.AspNetCore.WebApi.RouteResolver;


namespace bndd_hypermedia_server;

public static class MimeTypes
{
    public const string APPLICATION_VND_SIREN_JSON = "application/vnd.siren+json";
}

public partial record CreateExamParameters(string Location, DateTimeOffset Date) : IHypermediaActionParameter;
public partial record EnterGradeParameters(string Student, string Grade) : IHypermediaActionParameter;

[HypermediaObject(Title = "Entry Point to the API. This is the only document for which you need to know the route.", Classes = new string[]{ "EntryPoint" })]
public partial class EntryPointHto : HypermediaObject
{
    public EntryPointHto(
        object? lecturesKey
    ) : base(hasSelfLink: true)
    {
        Links.Add("Lectures", new HypermediaObjectKeyReference(typeof(LecturesHto), lecturesKey));
    }
}

[HypermediaObject(Title = "Collection of all lectures.", Classes = new string[]{ "Lectures" })]
public partial class LecturesHto : HypermediaObject
{
    public LecturesHto(
        IEnumerable<LectureHto> lecture
    ) : base(hasSelfLink: true)
    {
        Entities.AddRange("Lecture", lecture);
    }
}

[HypermediaObject(Title = "A lecture", Classes = new string[]{ "Lecture" })]
public partial class LectureHto : HypermediaObject
{
    [Key("id")]
    public string Id { get; set; }

    public string Name { get; set; }

    public int Year { get; set; }

    public string Lecturer { get; set; }

    [HypermediaAction(Name = "CreateExam", Title = "")]
    public CreateExamOp CreateExam { get; init; }

    public LectureHto(
        string id,
        string name,
        int year,
        string lecturer,
        CreateExamOp createExam,
        bool hasExam,
        object? examKey
    ) : base(hasSelfLink: true)
    {
        this.Id = id;
        this.Name = name;
        this.Year = year;
        this.Lecturer = lecturer;
        this.CreateExam = createExam;
        if (hasExam)
        {
            Links.Add("Exam", new HypermediaObjectKeyReference(typeof(ExamHto), examKey));
        }
    }
    public static object CreateKeyObject(string id) => new { id = id };

    public partial class CreateExamOp : HypermediaAction<CreateExamParameters>
    {
        public CreateExamOp(Func<bool> canExecuteCreateExam, CreateExamParameters? prefilledValues = default)
            : base(canExecuteCreateExam, prefilledValues) { }
    }
}

[HypermediaObject(Title = "The description of an exam for a lecture", Classes = new string[]{ "Exam" })]
public partial class ExamHto : HypermediaObject
{
    [Key("lecture")]
    [FormatterIgnoreHypermediaProperty]
    public string Lecture { get; set; }

    public string? Location { get; set; }

    public DateTimeOffset? Date { get; set; }

    [HypermediaAction(Name = "Reschedule", Title = "")]
    public RescheduleOp Reschedule { get; init; }

    [HypermediaAction(Name = "EnterGrade", Title = "")]
    public EnterGradeOp EnterGrade { get; init; }

    public ExamHto(
        string lecture,
        string? location,
        DateTimeOffset? date,
        RescheduleOp reschedule,
        EnterGradeOp enterGrade,
        object? lectureKey,
        bool hasGrade,
        object? gradeKey
    ) : base(hasSelfLink: true)
    {
        this.Lecture = lecture;
        this.Location = location;
        this.Date = date;
        this.Reschedule = reschedule;
        this.EnterGrade = enterGrade;
        Links.Add("Lecture", new HypermediaObjectKeyReference(typeof(LectureHto), lectureKey));
        if (hasGrade)
        {
            Links.Add("Grade", new HypermediaObjectKeyReference(typeof(GradeHto), gradeKey));
        }
    }
    public static object CreateKeyObject(string lecture) => new { lecture = lecture };

    public partial class RescheduleOp : HypermediaAction<CreateExamParameters>
    {
        public RescheduleOp(Func<bool> canExecuteReschedule, CreateExamParameters? prefilledValues = default)
            : base(canExecuteReschedule, prefilledValues) { }
    }

    public partial class EnterGradeOp : HypermediaAction<EnterGradeParameters>
    {
        public EnterGradeOp(Func<bool> canExecuteEnterGrade, EnterGradeParameters? prefilledValues = default)
            : base(canExecuteEnterGrade, prefilledValues) { }
    }
}

[HypermediaObject(Title = "The grade for an exam", Classes = new string[]{ "Grade" })]
public partial class GradeHto : HypermediaObject
{
    [Key("lecture")]
    [FormatterIgnoreHypermediaProperty]
    public string Lecture { get; set; }

    [Key("student")]
    [FormatterIgnoreHypermediaProperty]
    public string Student { get; set; }

    public string Grade { get; set; }

    public GradeHto(
        string lecture,
        string student,
        string grade
    ) : base(hasSelfLink: true)
    {
        this.Lecture = lecture;
        this.Student = student;
        this.Grade = grade;
    }
    public static object CreateKeyObject(string lecture, string student) => new { lecture = lecture, student = student };
}
