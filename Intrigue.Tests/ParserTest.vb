
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Intrigue


<TestClass()> Public Class ParserTest

    <TestMethod()> Public Sub ParserNode_ToString()

        Assert.AreEqual(
            "1",
            (New Intrigue.AtomNode(1)).ToString)
        Assert.AreEqual(
            "(1 2 3)",
            (New Intrigue.ListNode(1, 2, 3)).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_strings()

        Assert.AreEqual(
            """xxx""",
            Intrigue.Parser.Parse("""xxx""").ToString)
        Assert.AreEqual(
            """x\""xx""",
            Intrigue.Parser.Parse("""x\""xx""").ToString)
        Assert.AreEqual(
            """ab cd""",
            Intrigue.Parser.Parse("""ab cd""").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_integers()

        Assert.AreEqual(
            "1",
            Intrigue.Parser.Parse("1").ToString)
        Assert.AreEqual(
            "12",
            Intrigue.Parser.Parse("12").ToString)
        Assert.AreEqual(
            "-12",
            Intrigue.Parser.Parse("-12").ToString)
        Assert.AreEqual(
            "-12" & vbCr & "3",
            Intrigue.Parser.Parse("-12" & vbCr & "3").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_lists()

        Assert.AreEqual(
            "(1 2 3)",
            Intrigue.Parser.Parse("(1 2 3)").ToString)
        Assert.AreEqual(
            "(1 ""nada niente"" 3)",
            Intrigue.Parser.Parse("(1 ""nada niente"" 3)").ToString)
        Assert.AreEqual(
            "(1 2 (3 4))",
            Intrigue.Parser.Parse("(1 2 (3 4))").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_multiline_lists()

        Assert.AreEqual(
            "(alpha 1 2 (""a"" ""b"") 3)" & vbCr & "(+ 4 5 6)",
            Intrigue.Parser.Parse(
                <string>
                    (alpha
                      1 2 # the meat
                      ("a"
                       "b")
                      3)
                    + 4 5 6
                </string>.Nodes.First.ToString.Trim
            ).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_symbols()

        Assert.AreEqual(
            "alpha",
            Intrigue.Parser.Parse("alpha").ToString)
        Assert.AreEqual(
            "(x y z)",
            Intrigue.Parser.Parse("x y z").ToString)
        Assert.AreEqual(
            "(x y z)",
            Intrigue.Parser.Parse("(x y z)").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_nobrackets()

        Assert.AreEqual(
            "(alpha 1 2 3)",
            Intrigue.Parser.Parse("alpha 1 2 3").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_multilines()

        Assert.AreEqual(
            "(alpha (1 2) 3)" & vbCr & "(bravo 4 5 6)",
            Intrigue.Parser.Parse(
                <string>
                    (alpha (1 2) 3)
                    (bravo 4 5 6)
                </string>.Nodes.First.ToString.Trim
            ).ToString)

        Assert.AreEqual(
            "(alpha 1 2 3)" & vbCr & "(bravo 4 5 6)",
            Intrigue.Parser.Parse(
                <string>
                    alpha 1 2 3
                    bravo 4 5 6
                </string>.Nodes.First.ToString.Trim
            ).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_multilines_2()

        Assert.AreEqual(
            "(define x (lambda (y) y))" & vbCr & "(bravo 4 5 6)",
            Intrigue.Parser.Parse(
                <string>
                    (define x (lambda (y) y))
                    bravo 4 5 6
                </string>.Nodes.First.ToString.Trim
            ).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_quoted_list()

        Assert.AreEqual(
            "(quote 1 2 3)",
            Intrigue.Parser.Parse("(quote 1 2 3)").ToString)
        Assert.AreEqual(
            "(quote 1 2 3)",
            Intrigue.Parser.Parse("'(1 2 3)").ToString)
        Assert.AreEqual(
            "(quote 1 2 3)",
            Intrigue.Parser.Parse("quote 1 2 3").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_empty_string()

        Dim n As Node = Intrigue.Parser.Parse("")

        Assert.AreEqual("", n.ToString)
        Assert.AreEqual(0, n.toListNode.Nodes.Count)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_empty_string_2()

        Dim n As Node = Intrigue.Parser.Parse(vbCr & vbCrLf & vbCr)

        Console.WriteLine(n.Inspect)

        Assert.AreEqual("", n.ToString)
        Assert.AreEqual(0, n.toListNode.Nodes.Count)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_comments()

        Dim n As Node = Intrigue.Parser.Parse("# nada")

        Console.WriteLine(n.Inspect)

        Assert.AreEqual("", n.ToString)
        Assert.AreEqual(0, n.toListNode.Nodes.Count)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_comments_multi()

        Assert.AreEqual(
            "(alpha 1 2 3)" & vbCr & "(bravo 4 5 6)",
            Intrigue.Parser.Parse(
                <string>
                    (alpha 1 2 3)
                    # nada
                    bravo 4 5 6
                </string>.Nodes.First.ToString.Trim
            ).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_comments_eol()

        Assert.AreEqual(
            "(alpha 1 2 3)" & vbCr & "(bravo 4 5 6)",
            Intrigue.Parser.Parse(
                <string>
                    (alpha 1 2 3) # nada
                    # nada
                    bravo 4 5 6 # nada
                </string>.Nodes.First.ToString.Trim
            ).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_raise_on_missing_parenthese()

        Dim e As Exception = Nothing

        Try
            Intrigue.Parser.Parse("quote 1 2 3)")
        Catch e
        End Try

        Assert.AreEqual("unbalanced parenthese", e.Message)
    End Sub
End Class
