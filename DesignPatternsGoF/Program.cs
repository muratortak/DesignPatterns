using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatternsGoF
{
    // When piecewise object construction is complicated,
    // provide an API for doing it succinctly.
    #region Builder Pattern
    //public class HtmlElement
    //{
    //    public string Name, Text;
    //    public List<HtmlElement> Elements = new List<HtmlElement>();
    //    private const int indentSize = 2;

    //    public HtmlElement()
    //    {

    //    }
    //    public HtmlElement(string name, string text)
    //    {
    //        if(name == null)
    //        {
    //            throw new ArgumentNullException(paramName: nameof(name));
    //        }

    //        if (text == null)
    //        {
    //            throw new ArgumentNullException(paramName: nameof(text));
    //        }

    //        this.Name = name;
    //        this.Text = text;
    //    }

    //    private string ToStringImpl(int indent)
    //    {
    //        var sb = new StringBuilder();
    //        var i = new string(' ', indentSize * indent);
    //        sb.AppendLine($"{i}<{Name}>");

    //        if(!string.IsNullOrWhiteSpace(Text))
    //        {
    //            sb.Append(new string(' ', indentSize * indent + 1));
    //            sb.AppendLine(Text);
    //        }

    //        foreach (var e in Elements)
    //        {
    //            sb.Append(e.ToStringImpl(indent + 1));
    //        }

    //        sb.AppendLine($"{i}</{Name}>");

    //        return sb.ToString();
    //    }

    //    public override string ToString()
    //    {
    //        return ToStringImpl(0);
    //    }
    //}

    //public class HtmlBuilder
    //{

    //    private readonly string rootName;
    //    HtmlElement root = new HtmlElement();

    //    public HtmlBuilder(string rootName)
    //    {
    //        this.rootName = rootName;
    //        root.Name = rootName;
    //    }

    //    public HtmlBuilder AddChild(string childName, string childText)
    //    {
    //        var e = new HtmlElement(childName, childText);
    //        root.Elements.Add(e);
    //        return this;
    //    }

    //    public override string ToString()
    //    {
    //        return root.ToString();
    //    }

    //    public void Clear()
    //    {
    //        root = new HtmlElement { Name = rootName };
    //    }
    //}

    #endregion

    #region Fluent Builder Inheritance with Recursive Generics
    //public class Person
    //{
    //    public string Name;
    //    public string Position;

    //    public class Builder : PersonJobBuilder<Builder>
    //    {

    //    }

    //    public static Builder New => new Builder();
    //    public override string ToString()
    //    {
    //        return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
    //    }
    //}

    //public abstract class PersonBuilder
    //{
    //    protected Person person = new Person();
    //    public Person Build()
    //    {
    //        return person;
    //    }
    //}

    //// class Foo : Bar<Foo>
    //public class PersonInfoBuilder<SELF> : PersonBuilder where SELF : PersonInfoBuilder<SELF>
    //{

    //    public SELF Called(string name)
    //    {
    //        person.Name = name;
    //        return (SELF)this;
    //    }
    //}

    //public class PersonJobBuilder<SELF> : PersonInfoBuilder<PersonJobBuilder<SELF>> where SELF : PersonJobBuilder<SELF>
    //{
    //    public SELF WorksAsA(string position)
    //    {
    //        person.Position = position;
    //        return (SELF) this;
    //    }
    //}
    #endregion

    #region Faceted Builder
    //public class Person
    //{
    //    public string StreetAddress, Postcode, City;

    //    public string CompanyName, Position;
    //    public int AnnualIncome;

    //    public override string ToString()
    //    {
    //        return $"{nameof(StreetAddress)}: {StreetAddress}, {nameof(Postcode)}: {Postcode}, {nameof(City)}: {City}, {nameof(CompanyName)}: {CompanyName}," +
    //            $"{nameof(Position)}: {Position}, { nameof(AnnualIncome)}: {AnnualIncome}";
    //    }
    //}

    //public class PersonBuilder // facade
    //{
    //    // reference!
    //    protected Person person = new Person();
    //    public PersonJobBuilder Works => new PersonJobBuilder(person);
    //    public PersonAddressBuilder Lives => new PersonAddressBuilder(person);

    //    public static implicit operator Person(PersonBuilder pb)
    //    {
    //        return pb.person;
    //    }
    //}

    //public class PersonAddressBuilder : PersonBuilder
    //{
    //    // might not work with a value type!
    //    public PersonAddressBuilder(Person person)
    //    {
    //        this.person = person;
    //    }

    //    public PersonAddressBuilder At(string streetAddress)
    //    {
    //        person.StreetAddress = streetAddress;
    //        return this;
    //    }

    //    public PersonAddressBuilder WithPostCode(string postcode)
    //    {
    //        person.Postcode = postcode;
    //        return this;
    //    }

    //    public PersonAddressBuilder In(string city)
    //    {
    //        person.City = city;
    //        return this;
    //    }
    //}

    //public class PersonJobBuilder : PersonBuilder
    //{
    //    public PersonJobBuilder(Person person)
    //    {
    //        this.person = person;
    //    }

    //    public PersonJobBuilder At(string companyName)
    //    {
    //        person.CompanyName = companyName;
    //        return this;
    //    }

    //    public PersonJobBuilder AsA(string position)
    //    {
    //        person.Position = position;
    //        return this;
    //    }

    //    public PersonJobBuilder Earning(int amount)
    //    {
    //        person.AnnualIncome = amount;
    //        return this;
    //    }
    //}
    #endregion

    #region Excersise
    public class Code
    {
        public string className;
        public string propName;
        public string propType;

        public Code()
        {

        }

        public Code(string classname, string propname, string propType)
        {
            if(classname == null)
            {
                throw new ArgumentNullException(paramName: nameof(classname));
            }
            if (propname == null)
            {
                throw new ArgumentNullException(paramName: nameof(propname));
            }

            this.className = classname;
            this.propName = propname;
            this.propType = propType;
        }
    }

    public class CodeBuilder
    {
        Code code = new Code();

        public CodeBuilder()
        {

        }

        public CodeBuilder(string className)
        {
            code.className = className;
        }

        public CodeBuilder addField(string propName, string propType)
        {
            code.propName = propName;
            code.propType = propType;
            return this;
        }
    }
    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            #region Builder Pattern Client
            //var hello = "hello";
            //var sb = new StringBuilder();
            //sb.Append("<p>");
            //sb.Append(hello);
            //sb.Append("</p>");
            //Console.WriteLine(sb);

            //var words = new[] { "hello", "world" };
            //sb.Clear();
            //sb.Append("<ul>");
            //foreach (var word in words)
            //{
            //    sb.AppendFormat("<li>{0}</li>", word);
            //}
            //sb.Append("</ul>");
            //Console.WriteLine(sb);

            //var builder = new HtmlBuilder("ul");
            //builder.AddChild("li", "hello").AddChild("li", "world");
            //Console.WriteLine(builder.ToString());
            #endregion

            #region Fluent Builder Inheritance with Recursive Generics Client
            //var person = Person.New
            //    .Called("Dimitri")
            //    .WorksAsA("quant")
            //    .Build();
            //Console.WriteLine(person);
            #endregion

            #region Faceted Builder Client
            //var pb = new PersonBuilder();
            //Person person = pb
            //    .Lives.At("123 London Road")
            //        .In("London")
            //        .WithPostCode("SW12C")
            //    .Works.At("Fabrikam")
            //        .AsA("Engineer")
            //        .Earning(123000);
            //Console.WriteLine(person);
            #endregion
        }
    }

   

}
