
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Intrigue
Imports Intrigue.Nodes
Imports Intrigue.Parsing


<TestClass()> Public Class ParserTest

    <TestMethod()> Public Sub ParserNode_ToString()

        Assert.AreEqual(
            "1",
            (New Intrigue.Nodes.AtomNode(1)).ToString)
        Assert.AreEqual(
            "(1 2 3)",
            (New Intrigue.Nodes.ListNode(1, 2, 3)).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_strings()

        Assert.AreEqual(
            """xxx""",
            Parser.Parse("""xxx""").ToString)
        Assert.AreEqual(
            """x\""xx""",
            Parser.Parse("""x\""xx""").ToString)
        Assert.AreEqual(
            """ab cd""",
            Parser.Parse("""ab cd""").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_integers()

        Assert.AreEqual(
            "1",
            Parser.Parse("1").ToString)
        Assert.AreEqual(
            "12",
            Parser.Parse("12").ToString)
        Assert.AreEqual(
            "-12",
            Parser.Parse("-12").ToString)
        Assert.AreEqual(
            "-12" & vbCr & "3",
            Parser.Parse("-12" & vbCr & "3").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_lists()

        Assert.AreEqual(
            "(1 2 3)",
            Parser.Parse("(1 2 3)").ToString)
        Assert.AreEqual(
            "(1 ""nada niente"" 3)",
            Parser.Parse("(1 ""nada niente"" 3)").ToString)
        Assert.AreEqual(
            "(1 2 (3 4))",
            Parser.Parse("(1 2 (3 4))").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_multiline_lists()

        Assert.AreEqual(
            "(alpha 1 2 (""a"" ""b"") 3)" & vbCr & "(+ 4 5 6)",
            Parser.Parse(
                Util.NewString(
                    <string>
                        (alpha
                          1 2 # the meat
                          ("a"
                           "b")
                          3)
                        + 4 5 6
                    </string>)).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_symbols()

        Assert.AreEqual(
            "alpha",
            Parser.Parse("alpha").ToString)
        Assert.AreEqual(
            "(x y z)",
            Parser.Parse("x y z").ToString)
        Assert.AreEqual(
            "(x y z)",
            Parser.Parse("(x y z)").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_nobrackets()

        Assert.AreEqual(
            "(alpha 1 2 3)",
            Parser.Parse("alpha 1 2 3").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_multilines()

        Assert.AreEqual(
            "(alpha (1 2) 3)" & vbCr & "(bravo 4 5 6)",
            Parser.Parse(
                Util.NewString(
                    <string>
                        (alpha (1 2) 3)
                        (bravo 4 5 6)
                    </string>)).ToString)

        Assert.AreEqual(
            "(alpha 1 2 3)" & vbCr & "(bravo 4 5 6)",
            Parser.Parse(
                Util.NewString(
                    <string>
                        alpha 1 2 3
                        bravo 4 5 6
                    </string>)).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_multilines_2()

        Assert.AreEqual(
            "(define x (lambda (y) y))" & vbCr & "(bravo 4 5 6)",
            Parser.Parse(
                Util.NewString(
                    <string>
                        (define x (lambda (y) y))
                        bravo 4 5 6
                    </string>)).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_quoted_list()

        Assert.AreEqual(
            "(quote 1 2 3)",
            Parser.Parse("(quote 1 2 3)").ToString)
        Assert.AreEqual(
            "(quote 1 2 3)",
            Parser.Parse("'(1 2 3)").ToString)
        Assert.AreEqual(
            "(quote 1 2 3)",
            Parser.Parse("quote 1 2 3").ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_empty_string()

        Dim n As Node = Parser.Parse("")

        Assert.AreEqual("", n.ToString)
        Assert.AreEqual(0, n.toListNode.Nodes.Count)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_empty_string_2()

        Dim n As Node = Parser.Parse(vbCr & vbCrLf & vbCr)

        'Console.WriteLine(n.Inspect)

        Assert.AreEqual("", n.ToString)
        Assert.AreEqual(0, n.toListNode.Nodes.Count)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_comments()

        Dim n As Node = Parser.Parse("# nada")

        'Console.WriteLine(n.Inspect)

        Assert.AreEqual("", n.ToString)
        Assert.AreEqual(0, n.toListNode.Nodes.Count)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_comments_multi()

        Assert.AreEqual(
            "(alpha 1 2 3)" & vbCr & "(bravo 4 5 6)",
            Parser.Parse(
                Util.NewString(
                    <string>
                        (alpha 1 2 3)
                        # nada
                        bravo 4 5 6
                    </string>)).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_comments_eol()

        Assert.AreEqual(
            "(alpha 1 2 3)" & vbCr & "(bravo 4 5 6)",
            Parser.Parse(
                Util.NewString(
                    <string>
                        (alpha 1 2 3) # nada
                        # nada
                        bravo 4 5 6 # nada
                    </string>)).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_raise_on_missing_parenthese()

        Dim e As Exception = Nothing

        Try
            Parser.Parse("quote 1 2 3)")
        Catch e
        End Try

        Assert.AreEqual("unbalanced parenthese at line 1 column 12", e.Message)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_lax_0()

        Assert.AreEqual(
            "(define (plus x y) (+ x y))" & vbCr & "(plus 5 6)",
            Parser.Parse(
                Util.NewString(
                    <string>
                        define # big change
                          plus x y
                          + x y
                        plus 5 6
                    </string>)).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_lax_1()

        Assert.AreEqual(
            "(define plus (lambda (x y) (+ x y)))" & vbCr & "(plus 1 2)",
            Parser.Parse(
                Util.NewString(
                    <string>
                        (define plus (lambda (x y) (+ x y)))
                        plus 1 2
                    </string>)).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_lax_2()

        Assert.AreEqual(
            "(define (double x) (+ x x))" & vbCr & "(double 7)",
            Parser.Parse(
                Util.NewString(
                    <string>
                        define (double x) (+ x x)
                        double 7
                    </string>)).ToString)
    End Sub

    <TestMethod()> Public Sub Parser_Parse_lax_3()

        Assert.AreEqual(
            "(a (b c) (d (e f) g))" & vbCr &
            "(h (i) ())" & vbCr &
            "j",
            Parser.Parse(
                Util.NewString(
                    <![CDATA[
                        a
                          b c
                          d
                            e f
                            g
                        h
                          (i)
                          ()
                        j
                    ]]>)).ToString)
    End Sub
End Class
