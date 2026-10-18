using Azure.Search.Documents.Indexes;

namespace Search.Infrastructure.Indexing;

// The shape of one document in the Azure AI Search index. The attributes define the index schema:
// searchable fields are full-text searched, filterable/facetable fields can be used in filters and counts.
public sealed class ProductIndexDocument
{
    // AI Search keys must be strings, so Guids are stored as text.
    [SimpleField(IsKey = true)]
    public string Id { get; set; } = null!;

    [SimpleField(IsFilterable = true, IsFacetable = true)]
    public string CategoryId { get; set; } = null!;

    [SimpleField(IsFilterable = true)]
    public string Sku { get; set; } = null!;

    [SearchableField(IsSortable = true)]
    public string Name { get; set; } = null!;

    [SearchableField]
    public string Description { get; set; } = null!;

    // AI Search has no decimal type; double is its closest numeric type.
    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public double Price { get; set; }
}
