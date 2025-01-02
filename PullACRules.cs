using System;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using FormWorks.Core;
using rri.Base;
using rri.fwd;

namespace FwdProcessor
{
    public class PullACRules
    {
        private readonly Catalog _catalog;
        private readonly ILogger<PullACRules> _logger;

        public PullACRules(Catalog catalog, ILogger<PullACRules> logger)
        {
            _catalog = catalog;
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
                Console.WriteLine($"Processing file: {args[0]}");
                using var fwd = new Fwd(args[0]);

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

                Console.WriteLine($"Release Number: {releaseNumber}, Release Date: {releaseDate}");
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
                Console.WriteLine($"Found {documentNames.Length} documents.");

                foreach (var docName in documentNames)
                {
                    Console.WriteLine($"Processing document: {docName}");
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
                foreach (var page in pages)
                {
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
            // Process configuration logic here
        }
    }
}
