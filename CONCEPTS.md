# Immutable Architecture

Immutability solves many problems in application development.
It let's the client retry without fear of duplication.
It tolerates messages passing one another and arriving out of order.
And it keeps caches from getting out of date.
It should be your default position.

## Alternate Keys

Most applications use an auto-incremented ID to identify each row.
They return IDs to the client and use them in URLs.
Clients send those IDs back to the server in follow-up commands.

Auto-incremented IDs are great for databases.
They are the most efficient clustered index because they prevent page-splitting.
They can be quickly indexed and used for foreign keys.
But they are terrible external identifiers.
A client has to wait on a server to allocate an identifier.
If the client retries that operation, the server might allocate two identifiers.

Use auto-incremented IDs as the primary key of the table.
But also give each table an alternate key.
The alternate key is the identifier used outside of the database.

### Select or Insert

When the user issues a command, they supply alternate keys.
First *SELECT* to find whether the row that they are refering to exists.
And then, if it doesn't, *INSERT* it.

The output of the Select or Insert operation is the ID of the row.
In Entity Framework, it's the entity object, which tracks the ID.
That ID should never leave the application, so the Select or Insert operation should be private.

### Preventing Duplicates

### Client-Produced Identifiers

## Snapshots

## Tombstones

## Content Addressable Storage

## Commutative Messages
