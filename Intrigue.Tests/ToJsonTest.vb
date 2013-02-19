
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class ToJsonTest

    <TestMethod()> Public Sub ToJson_array()

        Assert.AreEqual(
            "[ 1, ""deux"", 3.0, [ 4, 5 ], [] ]",
            Interpreter.DoEval("'(1 ""deux"" 3.0 (4 5) ())").ToJson)
    End Sub

    <TestMethod()> Public Sub ToJson_object()
        Assert.AreEqual(
            "{ ""name"": ""Kiyomori"", ""age"": [ 33, 55, ""douze"" ] }",
            Interpreter.DoEval(Util.NewString(
                <![CDATA[
                    { "name": "Kiyomori", "age": [ 33, 55, "douze" ] }
                ]]>
            )).ToJson)
    End Sub

    <TestMethod()> Public Sub ToJson_modified_object()

        Assert.AreEqual(
            "{ ""age"": 55, ""name"": ""Kiyomori"" }",
            Interpreter.DoEval(Util.NewString(
                <![CDATA[
                    define user { "name": "Kiyomori", "age": 33 }
                    user 'set "age" 55
                    user
                ]]>
            )).ToJson)
    End Sub
End Class
