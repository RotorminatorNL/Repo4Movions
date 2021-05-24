﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Application
{
    public class AdminMovieModel
    {
        [JsonPropertyName("id")]
        [Required]
        public int ID { get; set; }

        [JsonPropertyName("description")]
        [Required]
        public string Description { get; set; }

        [JsonPropertyName("length")]
        [Required]
        public int Length { get; set; }

        [JsonPropertyName("releaseDate")]
        [Required]
        public DateTime ReleaseDate { get; set; }

        [JsonPropertyName("title")]
        [Required]
        public string Title { get; set; }

        [JsonPropertyName("companies")]
        public IEnumerable<AdminCompanyModel> Companies { get; set; }

        [JsonPropertyName("crew")]
        public IEnumerable<AdminCrewMemberModel> Crew { get; set; }

        [JsonPropertyName("genres")]
        public IEnumerable<AdminGenreModel> Genres { get; set; }

        [JsonPropertyName("language")]
        [Required]
        public AdminLanguageModel Language { get; set; }
    }
}
