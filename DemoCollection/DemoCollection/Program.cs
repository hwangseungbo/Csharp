using System;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace DemoCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Read();  //콘솔에서 한 문자를 정수로 입력받습니다.
            //Console.ReadLine(); //콘솔에서 한 줄을 입력받습니다. 또 콘솔 앱 프로그램에는 현재 시점에서 잠시 멈추는 기능이 있어 엔터키를 누를 때까지 대기합니다.
            //Console.ReadKey();  //콘솔에서 다음 문자나 사용자가 누른 기능 키를 가져옵니다.
            //Console.WriteLine(Console.ReadLine());    //103p 콘솔에서 받은 문자열을 그대로 콘솔에 출력

            //Console.WriteLine("이름을 입력하시오 => ");   //104p 콘솔에서 이름을 입력받아 출력
            //string name = Console.ReadLine();
            //Console.WriteLine("안녕하세요. {0}님.", name);

            //int x = Console.Read(); //105p 메서드로 문자 하나를 정수로 입력받기
            //Console.WriteLine(x);   //A를 입력했다면 A에 해당하는 정수 값 65출력
            //Console.WriteLine(Convert.ToChar(x));   //65에 해당하는 유니코드 문자 출력

            //long l = long.MaxValue;
            //Console.WriteLine($"l의 값 : {l}");
            //int i = (int)l;
            //Console.WriteLine($"i의 값 : {i}");

            //double d = 12.34;
            //int i = 1234;
            //d = i;
            //Console.WriteLine("암시적 형변환 = "+d);
            //d = 12.34;
            //i = (int)d;
            //Console.WriteLine("명시적 형변환 = "+i);
            //string s = "";
            //s = Convert.ToString(d);
            //Console.WriteLine("형식 변환 = "+s);

            //int i = 1234;
            //string s = "안녕하세요";
            //char c = 'A';
            //double d = 3.14;
            //object o = new object();    //개체 : 개체를 생성하는 구문
            //Console.WriteLine(i.GetType());
            //Console.WriteLine(s.GetType());
            //Console.WriteLine(c.GetType());
            //Console.WriteLine(d.GetType());
            //Console.WriteLine(o.GetType());

            //Console.Write("정수를 입력하세요 : ");
            //string input = Console.ReadLine();
            //int number = Convert.ToInt32(input);
            //Console.WriteLine($"{number}-{number.GetType()}");

            //Console.Write("실수를 입력하세요 : ");
            //string input = Console.ReadLine();
            //double PI = Convert.ToDouble(input);
            //Console.WriteLine(PI);

            //Console.WriteLine("문자열을 입력하세요 : ");
            //string input = Console.ReadLine();
            //char c = Convert.ToChar(input);
            //Console.WriteLine(c);

            //byte x = 10;
            //Console.WriteLine(
            //    $"십진수: {x} -> 이진수: {Convert.ToString(x,2).PadLeft(8,'0')}");

            //var name = "C#";
            //Console.WriteLine(name);
            //var version = 8.0;
            //Console.WriteLine("{0:0.0}",version);

            //var s = Console.ReadLine();
            //var c = Convert.ToChar(Console.Read());
            //Console.WriteLine($"{s} : {s.GetType()}, {c} : {c.GetType()}");

            //Console.WriteLine("아무키나 누르세요.");
            //ConsoleKeyInfo cki = Console.ReadKey(true);
            //Console.WriteLine("{0}", cki.Key);
            //Console.WriteLine("{0}", cki.KeyChar);
            //Console.WriteLine("{0}", cki.Modifiers);
            //if (cki.Key == ConsoleKey.Q)
            //{
            //    Console.WriteLine("Q를 입력하셨군요...");
            //}

            //Console.WriteLine($"!ture  -> {!true}");
            //Console.WriteLine($"!false -> {!false}");
            //var x = Convert.ToInt32("1010", 2);
            //var y = Convert.ToInt32("0110", 2);
            //var and = x & y;
            //Console.WriteLine($"{and} : {Convert.ToString(and, 2)}");
            //var or = x | y;
            //Console.WriteLine($"{or} : {Convert.ToString(or, 2)}");
            //var xor = x ^ y;
            //Console.WriteLine($"{xor} : {Convert.ToString(xor, 2)}");
            //var not = -x;
            //Console.WriteLine($"{not} : {Convert.ToString(not, 2)}");

            //int kor = 100;
            //int eng = 90;
            //int tot = 0;
            //double avg = 0.0;
            //tot = kor + eng;
            //avg = tot / 2.0;
            //Console.WriteLine("총점 : {0}", tot);
            //Console.WriteLine("평균 : {0}", avg);

            //int score = 60;
            //if(score >= 60)
            //{
            //    Console.WriteLine("합격");
            //}

            //bool bln = false;
            //if(!bln)
            //{
            //    Console.WriteLine("bln : false -> ! -> true");
            //}

            //Console.WriteLine("영문 대문자또는 소문자 하나를 입력하세요.");
            //char c = Convert.ToChar(Console.ReadLine());
            //if(c >='A' && c <= 'Z')
            //{
            //    Console.WriteLine($"{c}는 대문자입니다.");
            //}
            //else
            //{
            //    Console.WriteLine($"{c}는 소문자입니다.");
            //}

            //Console.WriteLine("문자를 입력하세요. (y/n/c) : ");
            //char input = Convert.ToChar(Console.ReadLine());
            //if(input == 'y')
            //{
            //    Console.WriteLine("Yes");
            //}
            //else
            //{
            //    if(input =='n')
            //    {
            //        Console.WriteLine("No");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Cancel");
            //    }
            //}

            //Console.Write("점수 : ");
            //int score = Convert.ToInt32(Console.ReadLine());
            //string grade = "";
            //if (score >= 90)
            //{
            //    grade = "금메달";
            //}
            //else
            //{
            //    if(score >= 80)
            //    {
            //        grade = "은메달";
            //    }
            //    else
            //    {
            //        if (score >= 70)
            //        {
            //            grade = "동메달";
            //        }
            //        else
            //        {
            //            grade = "노메달";
            //        }
            //    }
            //}
            //Console.WriteLine($"{grade}을 수상했습니다.");

            //Console.WriteLine("정수 입력 : _\b");
            //int a = Convert.ToInt32(Console.ReadLine());

            //if(a%2 != 0)
            //{
            //    Console.WriteLine("홀수");
            //}
            //else
            //{
            //    Console.WriteLine("짝수");
            //}

            //string data = "1234";
            //int result;
            //if (int.TryParse(data ,out result))
            //{
            //    Console.WriteLine("변환 가능 : {0}", result);
            //}
            //else
            //{
            //    Console.WriteLine("변환 불가");
            //}

            //int x = 2;
            //switch (x)
            //{
            //    case 1:
            //        Console.WriteLine("1입니다.");
            //        break;
            //    case 2:
            //        Console.WriteLine("2입니다.");
            //        break;
            //}

            //Console.WriteLine("정수를 입력하세요");
            //int answer = Convert.ToInt32(Console.ReadLine());dd
            //switch (answer)
            //{
            //    case 1:
            //        Console.WriteLine("1을 선택하셨습니다.");
            //        break;
            //    case 2:
            //        Console.WriteLine("2을 선택하셨습니다.");
            //        break;
            //    case 3:
            //        Console.WriteLine("3을 선택하셨습니다.");
            //        break;
            //    default :
            //        Console.WriteLine("그냥 찍으셨군요.");
            //        break;
            //}

            //for ( ; ; )
            //{
            //    Console.WriteLine("무한 루프");
            //}

            //string str = "ABC123";
            //foreach(char c in str)
            //{
            //    Console.Write($"{c}\t"); 
            //}
            //Console.WriteLine();

            ////int[] intArray;
            ////intArray = new int[3];
            ////intArray[0] = 1;
            ////intArray[1] = 2;
            ////intArray[2] = 3;
            //int[] intArray = new int[] { 1, 2, 3 };
            //for (int i = 0; i < 3; i++)
            //{
            //    Console.WriteLine($"{i}번째 인덱스 : {intArray[i]}");
            //}

            //t[] intArray = { 0, 1, 2 };

            //int[] numbers = { 1, 1_000, 10_000, 1_000_000 };
            //foreach (int number in numbers)
            //{
            //    Console.WriteLine(number);
            //}

            //Console.WriteLine("renew 10/12");

        }
    }
}
