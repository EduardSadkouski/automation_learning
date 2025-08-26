using FluentAssertions;
using NUnit.Framework;
using Task1File = Task1.SourceCode.File;
using System.Reflection;

namespace Task1.Tests
{
    [TestFixture]
    public class FileTests
    {
        [Test]
        public void CreateFileWithValidNameAndContent()
        {
            // TC-1 | Create file with valid name and content
            var file = new Task1File("doc.txt", "hello");
            file.GetFileName().Should().Be("doc.txt");
            file.GetSize().Should().Be(2); // half of 5, int division
            var extension = typeof(Task1File)
                .GetField("_extension", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(file);
            extension.Should().Be("txt"); // expect only extension
        }

        [Test]
        public void GetFileName_ShouldReturnFileName()
        {
            // TC-2 | Get filename from file
            var file = new Task1File("doc.txt", "hello");
            file.GetFileName().Should().Be("doc.txt");
        }

        [Test]
        public void GetSizeWithEvenContent()
        {
            // TC-3 | Get size from file with even length content
            var file = new Task1File("data.txt", "abcd"); // length = 4
            file.GetSize().Should().Be(2);
        }

        [Test]
        public void GetExtensionFromArchive()
        {
            // TC-4 | File extension parsing should work correctly
            var file = new Task1File("archive.tar.gz", "content");
            var extension = typeof(Task1File)
                .GetField("_extension", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(file);
            extension.Should().Be("gz"); // expect only extension
        }

        [Test]
        public void GetSizeWithOddContent()
        {
            // TC-5 | File size should be calculated with decimals
            var file = new Task1File("abc.txt", "abc"); // length = 3
            file.GetSize().Should().Be(1); // int division
        }
    }
}