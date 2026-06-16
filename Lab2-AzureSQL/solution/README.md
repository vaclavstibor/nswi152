# Solution of Lab 2 - Azure SQL & Azure WebJob

## Screenshots

### DB Table `EmailQueue`:
![DB Table EmailQueue](img/db_table_emailqueue.png)

### WebJob Overview:
![Web Jobs Overview](img/web_jobs_overview.png)

### WebJob Dashboard:
![Web Jobs Dashboard](img/web_jobs_dashboard.png)

### WebJob Logs:
![Logs](img/logs.png)

### SQL Explorer - Recepient Created Sent:
![SQL Explorer Recipient Created Sent](img/sql_explorer_recipient_created_sent.png)

## Summary
- Vytvoren projekt WebJob (.NET 6 Console) s periodickym ctenim tabulky `EmailQueue`.
- WebJob odesila e-maily pres SMTP a oznacuje zaznamy jako odeslane (`Sent`).
- V Azure vytvorena SQL Database + SQL Server a pridana pravidla firewallu.
- WebJob publikovan do Azure App Service (z Lab1) jako *Continuous* job.