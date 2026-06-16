# Solution of Lab 10 – Azure Networking

## Screenshots

### Bootstrap a ověření (`azure_networking_scripts`):
![Resource group rg-aznet-lab](img/bootstrap_rg.png)

![Subnety vnet-app](img/vnet_app_subnets.png)

![Subnety vnet-data](img/vnet_data_subnets.png)

Bootstrap a finální ověření lokálně přes Azure CLI (`./00-bootstrap.sh`, `./01-verify.sh`). Region **`germanywestcentral`** — `westeurope` blokovaný na Azure for Students. Výstup verify: [`terminal_output.txt`](terminal_output.txt).

### Blob před Private Link (veřejný přístup ještě funguje):
![Blob URL v prohlížeči – před vypnutím](img/test_blob_web.png)

### VNet peering (`vnet-app` ↔ `vnet-data`):
![AppToData-peering Connected](img/app_to_data_peering.png)

### NSG na `nsg-data` (Allow 443 + Deny zbytek):
![Inbound security rules](img/nsg_rules.png)

### Private Endpoint pro blob storage:
![Vytvoření Private Endpoint – průvodce](img/private_endpoint_for_blob_storage.png)

![Private Endpoint – Overview](img/pe_storage_blob.png)

![NIC pe-storage-blob-nic – privátní IP 10.10.2.4](img/pe_storage_blob_nic.png)

![Private DNS zone – Overview](img/privat_link_blob_core_windows_net_overview.png)

![Private DNS zone – Virtual network links](img/private_link_blob_core_windows_net_virtual.png)

![Private DNS – Recordsets, A záznam aznetlabfc682e](img/dns_management.png)

### Zákaz veřejného přístupu ke Storage:
![Public network access Disabled](img/disbale_network_access.png)

![Blob URL – AuthorizationFailure](img/test_blob_web_disabled.png)

*(Prohlížeč může mít blob v cache; po Disable vrací Azure 403 — ověřeno i přes `curl`.)*

## Summary

- Bootstrap: RG **`rg-aznet-lab`**, VNety **`vnet-app`** / **`vnet-data`**, NSG, Storage **`aznetlabfc682e`**, demo blob, Private DNS zone — region **Germany West Central**.
- **VNet peering** `AppToData-peering` — stav **Connected**.
- **NSG** `nsg-data`: Allow TCP 443 ze `10.10.1.0/24`, Deny zbytek (prio 200 / 300).
- **Private Endpoint** `pe-storage-blob` v `snet-private`; DNS A záznam → **`10.10.2.4`**.
- **Public network access** na Storage **Disabled** — blob z internetu nejde (`AuthorizationFailure`).

Po kurzu: `99-cleanup.sh` nebo smazat RG `rg-aznet-lab`.
