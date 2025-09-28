using GlobalRightsManagement.Core;

namespace GlobalRightsManagement.Tests
{
    public class GlobalRightsManagementTests
    {
        private const string ContractsFileContent =
            "Artist|Title|Usages|StartDate|EndDate\n" +
            "Tinie Tempah|Frisky (Live from SoHo)|digital download, streaming|02-01-2012|\n" +
            "Tinie Tempah|Miami 2 Ibiza|digital download|02-01-2012|\n" +
            "Tinie Tempah|Till I'm Gone|digital download|08-01-2012|\n" +
            "Monkey Claw|Black Mountain|digital download|02-01-2012|\n" +
            "Monkey Claw|Iron Horse|digital download, streaming|06-01-2012|\n" +
            "Monkey Claw|Motor Mouth|digital download, streaming|03-01-2011|\n" +
            "Monkey Claw|Christmas Special|streaming|12-25-2012|12-31-2012\n";

        private const string PartnersFileContent =
            "Partner|Usage\n" +
            "ITunes|digital download\n" +
            "YouTube|streaming\n";

        private static (string contractsPath, string partnersPath) WriteTempReferenceFiles()
        {
            var tmpDir = Path.Combine(Path.GetTempPath(), "grm_tests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tmpDir);

            var contractsPath = Path.Combine(tmpDir, "music_contracts.txt");
            var partnersPath = Path.Combine(tmpDir, "distribution_partners.txt");

            File.WriteAllText(contractsPath, ContractsFileContent);
            File.WriteAllText(partnersPath, PartnersFileContent);

            return (contractsPath, partnersPath);
        }

        private static void Cleanup((string contractsPath, string partnersPath) f)
        {
            try
            {
                if (File.Exists(f.contractsPath)) File.Delete(f.contractsPath);
                if (File.Exists(f.partnersPath)) File.Delete(f.partnersPath);
                var dir = Path.GetDirectoryName(f.contractsPath);
                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir)) Directory.Delete(dir, true);
            }
            catch
            {
                
            }
        }

        [Fact]
        public void Scenario_ITunes_2012_03_01()
        {
            var files = WriteTempReferenceFiles();
            try
            {
                var effective = new DateOnly(2012, 3, 1);
                var results = GRMQueryService.Query(files.contractsPath, files.partnersPath, "ITunes", effective).ToList();

                Assert.Equal(4, results.Count);

                foreach (var r in results)
                {
                    var usage = r.Usages.Single(); 
                    Assert.Equal("digital download", usage, ignoreCase: true);
                }

                Assert.Collection(results,
                    r => {
                        Assert.Equal("Monkey Claw", r.Artist);
                        Assert.Equal("Black Mountain", r.Title);
                        Assert.Equal(new DateOnly(2012, 2, 1), r.StartDate);
                        Assert.Null(r.EndDate);
                    },
                    r => {
                        Assert.Equal("Monkey Claw", r.Artist);
                        Assert.Equal("Motor Mouth", r.Title);
                        Assert.Equal(new DateOnly(2011, 3, 1), r.StartDate);
                        Assert.Null(r.EndDate);
                    },
                    r => {
                        Assert.Equal("Tinie Tempah", r.Artist);
                        Assert.Equal("Frisky (Live from SoHo)", r.Title);
                        Assert.Equal(new DateOnly(2012, 2, 1), r.StartDate);
                        Assert.Null(r.EndDate);
                    },
                    r => {
                        Assert.Equal("Tinie Tempah", r.Artist);
                        Assert.Equal("Miami 2 Ibiza", r.Title);
                        Assert.Equal(new DateOnly(2012, 2, 1), r.StartDate);
                        Assert.Null(r.EndDate);
                    }
                );
            }
            finally
            {
                Cleanup(files);
            }
        }

        [Fact]
        public void Scenario_YouTube_2012_12_27()
        {
            var files = WriteTempReferenceFiles();
            try
            {
                var effective = new DateOnly(2012, 12, 27);
                var results = GRMQueryService.Query(files.contractsPath, files.partnersPath, "YouTube", effective).ToList();

                Assert.Equal(4, results.Count);

                foreach (var r in results)
                {
                    var usage = r.Usages.Single();
                    Assert.Equal("streaming", usage, ignoreCase: true);
                }

                Assert.Collection(results,
                    r => {
                        Assert.Equal("Monkey Claw", r.Artist);
                        Assert.Equal("Christmas Special", r.Title);
                        Assert.Equal(new DateOnly(2012, 12, 25), r.StartDate);
                        Assert.Equal(new DateOnly(2012, 12, 31), r.EndDate);
                    },
                    r => {
                        Assert.Equal("Monkey Claw", r.Artist);
                        Assert.Equal("Iron Horse", r.Title);
                        Assert.Equal(new DateOnly(2012, 6, 1), r.StartDate);
                        Assert.Null(r.EndDate);
                    },
                    r => {
                        Assert.Equal("Monkey Claw", r.Artist);
                        Assert.Equal("Motor Mouth", r.Title);
                        Assert.Equal(new DateOnly(2011, 3, 1), r.StartDate);
                        Assert.Null(r.EndDate);
                    },
                    r => {
                        Assert.Equal("Tinie Tempah", r.Artist);
                        Assert.Equal("Frisky (Live from SoHo)", r.Title);
                        Assert.Equal(new DateOnly(2012, 2, 1), r.StartDate);
                        Assert.Null(r.EndDate);
                    }
                );
            }
            finally
            {
                Cleanup(files);
            }
        }

        [Fact]
        public void Scenario_YouTube_2012_04_01()
        {
            var files = WriteTempReferenceFiles();
            try
            {
                var effective = new DateOnly(2012, 4, 1);
                var results = GRMQueryService.Query(files.contractsPath, files.partnersPath, "YouTube", effective).ToList();

                Assert.Equal(2, results.Count);

                foreach (var r in results)
                {
                    var usage = r.Usages.Single();
                    Assert.Equal("streaming", usage, ignoreCase: true);
                }

                Assert.Collection(results,
                    r => {
                        Assert.Equal("Monkey Claw", r.Artist);
                        Assert.Equal("Motor Mouth", r.Title);
                        Assert.Equal(new DateOnly(2011, 3, 1), r.StartDate);
                        Assert.Null(r.EndDate);
                    },
                    r => {
                        Assert.Equal("Tinie Tempah", r.Artist);
                        Assert.Equal("Frisky (Live from SoHo)", r.Title);
                        Assert.Equal(new DateOnly(2012, 2, 1), r.StartDate);
                        Assert.Null(r.EndDate);
                    }
                );
            }
            finally
            {
                Cleanup(files);
            }
        }

    }
}