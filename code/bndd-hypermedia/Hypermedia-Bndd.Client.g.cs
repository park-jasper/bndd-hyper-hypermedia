#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using RESTyard.Client;
using RESTyard.Client.Builder;
using RESTyard.Client.Hypermedia;
using RESTyard.Client.Hypermedia.Attributes;
using RESTyard.Client.Hypermedia.Commands;


namespace bndd_hypermedia_client;

public class DefaultHypermediaClientBuilder
{
    public static IHypermediaResolverBuilder CreateBuilder()
        => HypermediaResolverBuilder.CreateBuilder()
            .ConfigureObjectRegister(register =>
            {
                register.Register<EntryPointHco>();
                register.Register<LecturesHco>();
                register.Register<LectureHco>();
                register.Register<ExamHco>();
                register.Register<GradeHco>();
            });
}
public partial record CreateExamParameters(string Location, DateTimeOffset Date);
public partial record EnterGradeParameters(string Student, string Grade);

[HypermediaClientObject("EntryPoint")]
public partial class EntryPointHco : HypermediaClientObject
{
    [Mandatory]
    [HypermediaRelations(new[]{ "self" })]
    public MandatoryHypermediaLink<EntryPointHco> Self { get; set; } = default!;

    [Mandatory]
    [HypermediaRelations(new[]{ "Lectures" })]
    public MandatoryHypermediaLink<LecturesHco> Lectures { get; set; } = default!;
}

[HypermediaClientObject("Lectures")]
public partial class LecturesHco : HypermediaClientObject
{
    [Mandatory]
    [HypermediaRelations(new[]{ "self" })]
    public MandatoryHypermediaLink<LecturesHco> Self { get; set; } = default!;

    [HypermediaRelations(new[]{ "Lecture" })]
    public List<LectureHco> Lecture { get; set; } = default!;
}

[HypermediaClientObject("Lecture")]
public partial class LectureHco : HypermediaClientObject
{
    [Mandatory]
    public string Id { get; set; } = default!;

    [Mandatory]
    public string Name { get; set; } = default!;

    [Mandatory]
    public int Year { get; set; } = default!;

    [Mandatory]
    public string Lecturer { get; set; } = default!;

    [Mandatory]
    [HypermediaRelations(new[]{ "self" })]
    public MandatoryHypermediaLink<LectureHco> Self { get; set; } = default!;

    [HypermediaRelations(new[]{ "Exam" })]
    public HypermediaLink<ExamHco>? Exam { get; set; } = default!;

    [HypermediaCommand("CreateExam")]
    public IHypermediaClientFunction<ExamHco, CreateExamParameters>? CreateExam { get; set; }
}

[HypermediaClientObject("Exam")]
public partial class ExamHco : HypermediaClientObject
{
    public string? Location { get; set; } = default!;

    public DateTimeOffset? Date { get; set; } = default!;

    [Mandatory]
    [HypermediaRelations(new[]{ "self" })]
    public MandatoryHypermediaLink<ExamHco> Self { get; set; } = default!;

    [Mandatory]
    [HypermediaRelations(new[]{ "Lecture" })]
    public MandatoryHypermediaLink<LectureHco> Lecture { get; set; } = default!;

    [HypermediaRelations(new[]{ "Grade" })]
    public HypermediaLink<GradeHco>? Grade { get; set; } = default!;

    [HypermediaCommand("Reschedule")]
    public IHypermediaClientFunction<ExamHco, CreateExamParameters>? Reschedule { get; set; }

    [HypermediaCommand("EnterGrade")]
    public IHypermediaClientFunction<GradeHco, EnterGradeParameters>? EnterGrade { get; set; }
}

[HypermediaClientObject("Grade")]
public partial class GradeHco : HypermediaClientObject
{
    [Mandatory]
    public string Grade { get; set; } = default!;

    [Mandatory]
    [HypermediaRelations(new[]{ "self" })]
    public MandatoryHypermediaLink<GradeHco> Self { get; set; } = default!;
}
