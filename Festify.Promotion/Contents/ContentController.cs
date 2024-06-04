﻿using Microsoft.AspNetCore.Mvc;

namespace Festify.Promotion.Contents;

public class ContentController : ControllerBase
{
    private readonly ContentQueries queries;

    public ContentController(ContentQueries queries)
    {
        this.queries = queries;
    }

    [HttpGet]
    [Route("Content/{hash}")]
    [ResponseCache(Duration = 60 * 60 * 24 * 365)]
    public async Task<IActionResult> Get(string hash)
    {
        var content = await queries.GetContent(Uri.UnescapeDataString(hash));
        if (content == null)
        {
            return NotFound();
        }
        else
        {
            return File(content.Binary, content.ContentType);
        }
    }
}