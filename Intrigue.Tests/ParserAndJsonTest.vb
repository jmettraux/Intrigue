
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Intrigue
Imports Intrigue.Nodes
Imports Intrigue.Parsing


<TestClass()> Public Class ParserAndJsonTest

    <TestMethod()> Public Sub Parser_json_arrays()

        Assert.AreEqual(
            "(make-jarray 1 2 3)",
            Parser.Parse("[ 1, 2, 3 ]").ToString)

        'Assert.AreEqual(
        '    "(let ((var0 0) (var1 1)) ((do-this var0) (do-that var1)))",
        '    Parser.Parse(
        '        Util.NewString(
        '            <![CDATA[
        '              let
        '                (
        '                  var0 0
        '                  var1 1
        '                (
        '                  do-this var0
        '                  do-that var1
        '            ]]>)).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_json_objects()

        Assert.AreEqual(
            "(make-jobject ""a"" 1 ""b"" 2)",
            Parser.Parse("{ ""a"": 1, ""b"": 2 }").ToString)
        Assert.AreEqual(
            "(make-jobject ""a"" 1 ""b"" 2)",
            Parser.Parse("{ ""a"": 1, ""b"": 2}").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_json_composites()

        Assert.AreEqual(
            "(make-jobject ""a"" 1 ""b"" (make-jarray 1 2 (make-jobject ""c"" ""nada"")))",
            Parser.Parse("{ ""a"": 1, ""b"": [ 1, 2, { ""c"": ""nada"" } ] }").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_json_and_errors()

        Dim e As Exception = Nothing

        Try
            Parser.Parse("[ ""a"": 1, ""b"": 2 ]")
        Catch e
        End Try

        Assert.AreEqual("Colons are not allowed in arrays", e.Message)

        e = Nothing

        Try
            Parser.Parse("( 1, 2 )")
        Catch e
        End Try

        Assert.AreEqual("Commas and colons are not allowed in plain lists", e.Message)
    End Sub
End Class
