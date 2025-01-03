using System;
using System.IO;
using Microsoft.Extensions.Logging;
using FormWorks.Core;
using rri.Base;
using rri.fwd;

namespace FwdProcessor
{
    public class PullACRules

        private readonly ILogger<PullACRules> _logger;

        public PullACRules(Catalog catalog, ILogger<PullACRules> logger)
        {
            _logger = logger;
        }

        public void Process(string[] args)
        {
            if (args == null || args.Length == 0 || !File.Exists(args[0]))
            {
                _logger.LogError("No valid file path provided.");
                Console.WriteLine("Error: Please provide a valid file path.");
                return;
            }

            try
            {
                _logger.LogInformation("Processing file: {FileName}", args[0]);
                using var fwd = new Fwd(args[0]);
                _logger.LogInformation("FWD file initialized successfully.");

                LogFWDAttributes(fwd);
                ProcessDocuments(fwd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during processing.");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void LogFWDAttributes(Fwd fwd)
        {
            try
            {
                var releaseNumber = fwd.GetFWDAttributes().GetInt("LastReleaseNumber");
                var releaseDate = fwd.GetFWDAttributes().GetString("ReleaseDateString");

                _logger.LogInformation("Release Number: {ReleaseNumber}, Release Date: {ReleaseDate}", releaseNumber, releaseDate);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to retrieve attributes.");
            }
        }

        private void ProcessDocuments(Fwd fwd)
        {
            try
            {
                var documentNames = fwd.GetDocumentNames();
                _logger.LogInformation("Found {DocumentCount} documents.", documentNames.Length);

                foreach (var docName in documentNames)
                {
                    _logger.LogInformation("Processing document: {DocumentName}", docName);
                    ProcessPages(fwd, docName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing documents.");
            }
        }

        private void ProcessPages(Fwd fwd, string docName)
        {
            try
            {
                var pages = fwd.GetPagesInDoc(docName);
                _logger.LogInformation("Found {PageCount} pages in document: {DocumentName}", pages.Length, docName);

                foreach (var page in pages)
                {
                    _logger.LogInformation("Processing page: {PageName}", page);
                    ProcessPageConfig(fwd, page);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing pages.");
            }
        }

        private void ProcessPageConfig(Fwd fwd, string pageName)
        {
            try
            {
                _logger.LogInformation("Processing configuration for page: {PageName}", pageName);
                // Add configuration processing logic here
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing page configuration: {PageName}", pageName);
            }
        }
    }
}
