// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Languages;
using Prometheus.Countries.Dto;
using Prometheus.Countries.Mappers;
using TUnit.Assertions.AssertConditions.Throws;

namespace Unit.Tests.Prometheus.Countries.Mappers;

public sealed class TranslationMapperTests
{
    private readonly IEnumerable<string> _languages = ["fra"];
    private readonly NameDto _name = new("Canada");

    [Test]
    public async Task AsDomainShouldMapDtoToDomain()
    {
        TranslationDto[] dto = [new("fra", "Canada")];

        Translation[] translations = dto.AsDomain(_name, _languages);

        using (Assert.Multiple())
        {
            Translation english = translations[0];

            await Assert.That(english.Language).IsEqualTo(Language.English);
            await Assert.That(english.Name).IsEqualTo("Canada");

            Translation french = translations[1];

            await Assert.That(french.Language).IsEqualTo(Language.French);
            await Assert.That(french.Name).IsEqualTo("Canada");
        }
    }

    [Test]
    public async Task AsDomainShouldExcludeTranslationsWhichAreNotInAcceptedLanguages()
    {
        TranslationDto[] dto = [new("deu", "Kanada")];

        Translation[] translations = dto.AsDomain(_name, _languages);

        await Assert.That(translations).HasSingleItem();
    }

    [Test]
    public async Task AsDomainShouldThrowNotSupportedExceptionForUnsupportedLanguage()
    {
        TranslationDto[] dto = [new("deu", "Kanada")];

        await Assert.That(() => dto.AsDomain(_name, ["deu"]))
                    .ThrowsExactly<NotSupportedException>()
                    .WithMessage("Language code 'deu' is not supported.");
    }
}
