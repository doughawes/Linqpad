<Query Kind="Statements">
  <Connection>
    <ID>ec85713e-b82f-4ddb-8718-46c5f0bda4c8</ID>
    <Persist>true</Persist>
    <Server>agl_hqenavdev</Server>
    <Database>ImportExport</Database>
    <ShowServer>true</ShowServer>
    <IncludeSystemObjects>true</IncludeSystemObjects>
  </Connection>
  <Output>DataGrids</Output>
</Query>

var dd = from r in INFORMATION_SCHEMA.COLUMNS where r.TABLE_NAME == "IntakeRecordError" orderby r.ORDINAL_POSITION select new { r.COLUMN_NAME, r.DATA_TYPE };
dd.Dump();


