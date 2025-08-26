using FluentAssertions;
using NUnit.Framework;
using Task1.SourceCode;
using Task1.SourceCode.exception;
using System.Reflection;

namespace Task1.Tests
{
    [TestFixture]
    public class FileStorageTests
    {
        [Test]
        public void CreateStorageWithDefaultSize()
        {
            // TC-1 | Create storage with default size
            var storage = new FileStorage();
            storage.GetFiles().Should().BeEmpty();
            var availableSize = (double)typeof(FileStorage)
                .GetField("_availableSize", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(storage);
            availableSize.Should().Be(100);
        }

        [Test]
        public void CreateStorageWithCustomSize()
        {
            // TC-2 | Create storage with custom size
            var storage = new FileStorage(200);
            storage.GetFiles().Should().BeEmpty();
            var availableSize = (double)typeof(FileStorage)
                .GetField("_availableSize", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(storage);
            availableSize.Should().Be(200); // BUG: implementation does +=
        }

        [Test]
        public void WriteFileWhenEnoughSpace()
        {
            // TC-3 | Successfully write a file into storage
            var storage = new FileStorage(100);
            var file = new Task1.SourceCode.File("doc.txt", "abcd"); // size = 2

            var result = storage.Write(file);

            result.Should().BeTrue();
            storage.IsExists("doc.txt").Should().BeTrue();
            var availableSize = (double)typeof(FileStorage)
                .GetField("_availableSize", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(storage);
            availableSize.Should().Be(98);
        }

        [Test]
        public void IsExistsShouldReturnTrueAfterWrite()
        {
            // TC-4 | Check file existence after writing
            var storage = new FileStorage(100);
            var file = new Task1.SourceCode.File("doc.txt", "abcd");
            storage.Write(file);

            storage.IsExists("doc.txt").Should().BeTrue();
        }

        [Test]
        public void DeleteFileWhenExists()
        {
            // TC-5 | Delete existing file
            var storage = new FileStorage();
            var file = new Task1.SourceCode.File("doc.txt", "abc");
            storage.Write(file);

            var result = storage.Delete("doc.txt");

            result.Should().BeTrue();
            storage.IsExists("doc.txt").Should().BeFalse();
        }

        [Test]
        public void GetFileWhenExists()
        {
            // TC-6 | Get file by name
            var storage = new FileStorage();
            var file = new Task1.SourceCode.File("doc.txt", "abc");
            storage.Write(file);

            var result = storage.GetFile("doc.txt");

            result.Should().NotBeNull();
            result.GetFileName().Should().Be("doc.txt");
        }

        [Test]
        public void CustomStorageSizeShouldNotIncreaseByDefault()
        {
            // TC-7 | Custom storage size should not be increased by default
            var storage = new FileStorage(200);
            var availableSize = (double)typeof(FileStorage)
                .GetField("_availableSize", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(storage);
            availableSize.Should().Be(200); // BUG: implementation gives 300
        }

        [Test]
        public void WriteFileWhenTooLarge()
        {
            // TC-8 | Write file larger than available space
            var storage = new FileStorage(50);
            var file = new Task1.SourceCode.File("big.txt", new string('a', 200)); // size = 100

            var result = storage.Write(file);

            result.Should().BeFalse();
        }

        [Test]
        public void WriteFileWhenEqualToAvailableSize()
        {
            // TC-9 | Write file equal to available size
            var storage = new FileStorage(10);
            var file = new Task1.SourceCode.File("fit.txt", new string('a', 20)); // size = 10

            var result = storage.Write(file);

            result.Should().BeTrue(); // BUG: code returns false (>= instead of >)
        }

        [Test]
        public void WriteFileWhenDuplicateName()
        {
            // TC-10 | Write file with duplicate name
            var storage = new FileStorage();
            var file1 = new Task1.SourceCode.File("doc.txt", "abc");
            var file2 = new Task1.SourceCode.File("doc.txt", "xyz");

            storage.Write(file1);
            var action = () => storage.Write(file2);

            action.Should().Throw<FileNameAlreadyExistsException>();
        }

        [Test]
        public void IsExistsShouldReturnFalseForSubstringMatch()
        {
            // TC-11 | Check existence with substring of filename
            var storage = new FileStorage();
            var file = new Task1.SourceCode.File("document.txt", "abc");
            storage.Write(file);

            storage.IsExists("doc").Should().BeFalse(); // BUG: implementation uses Contains
        }

        [Test]
        public void GetFileWhenNotExists()
        {
            // TC-12 | Get non-existent file
            var storage = new FileStorage();
            var result = storage.GetFile("ghost.txt");
            result.Should().BeNull();
        }

        [Test]
        public void DeleteFileWhenNotExists()
        {
            // TC-13 | Delete non-existent file
            var storage = new FileStorage();
            var result = storage.Delete("ghost.txt");
            result.Should().BeFalse();
        }
    }
}
