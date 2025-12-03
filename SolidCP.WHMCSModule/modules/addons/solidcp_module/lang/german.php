<?php if (!defined('WHMCS')) exit('ACCESS DENIED');
// Copyright (c) 2023, FuseCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.


/**
 * FuseCP WHMCS Server Module Client Area Language
 *
 * @author FuseCP
 * @link https://fusecp.com/
 * @access public
 * @name FuseCP
 * @version 1.1.4
 * @package WHMCS
 */
$_ADDONLANG['FuseCP_migration'] = 'Migration';
$_ADDONLANG['FuseCP_settings'] = 'Einstellungen';
$_ADDONLANG['FuseCP_addon_automation'] = 'Addon-Automatisierung';
$_ADDONLANG['FuseCP_configurable_options'] = 'Konfigurierbare Optionen';
$_ADDONLANG['FuseCP_sync_automation'] = 'Sync-Automatisierung';
$_ADDONLANG['FuseCP_close'] = 'Schließen';
$_ADDONLANG['FuseCP_yes'] = 'Ja';
$_ADDONLANG['FuseCP_no'] = 'Nein';
$_ADDONLANG['FuseCP_action'] = 'Aktion';
$_ADDONLANG['FuseCP_checkagain'] = 'Nochmal prüfen!';
$_ADDONLANG['FuseCP_addonsnotactive'] = 'Addon-Automatisierung ist nicht aktiviert. Bitte aktivieren Sie es zuerst auf der "Einstellungen"-Seite.';
$_ADDONLANG['FuseCP_configurablenotactive'] = 'Automatisierung der konfigurierbaren Optionen ist nicht aktiviert. Bitte aktivieren Sie es zuerst auf der "Einstellungen"-Seite.';
$_ADDONLANG['FuseCP_syncnotactive'] = 'Sync-Automatisierung ist nicht aktiviert. Bitte aktivieren Sie es zuerst auf der "Einstellungen"-Seite.';
$_ADDONLANG['FuseCP_sync_nosetting'] = 'Sync-Automatisierung besitzt aktuell keine Einstellungen.';

$_ADDONLANG['FuseCP_migration_needed'] = 'Eine Migration ist notwendig, bevor Sie das Modul nutzen können!';
$_ADDONLANG['FuseCP_migrateTable_text'] = 'Inhalt der Datenbanktabelle von "%s" zu "%s" kopieren.';
$_ADDONLANG['FuseCP_migrateDbValues_text'] = 'Wert "%s" in der Tabelle "%s" muss aufgrund von Strukturänderungen migriert werden.';
$_ADDONLANG['FuseCP_deactivateModules_text'] = 'Das Modul "%s" muss manuell deaktiviert werden, nachdem die vorherigen Schritte erledigt wurden!';
$_ADDONLANG['FuseCP_deleteFiles_text'] = 'Datei "%s" muss manuell via FTP gelöscht werden, nachdem alle vorherigen Schritte erledigt wurden!';
$_ADDONLANG['FuseCP_confirmdelete'] = 'Löschen bestätigen';
$_ADDONLANG['FuseCP_confirmdelete_long'] = 'Sind Sie sicher, dass Sie die Tabelle löschen möchten? Alle Werte in dieser Tabelle werden gelöscht. Dieser Vorgang ist nach Ihrer Bestätigun nicht mehr rückgängig zu machen!';
$_ADDONLANG['FuseCP_copytable'] = 'Alte Tabelle "%s" zur neuen "%s" kopieren';
$_ADDONLANG['FuseCP_deletetable'] = 'Alte Tabelle "%s" löschen (nicht empfohlen)';
$_ADDONLANG['FuseCP_migratedbvalues'] = 'Werte in der Tabelle "%s" zur neuen Struktur migrieren';
$_ADDONLANG['FuseCP_deactivatemodule'] = 'Gehen Sie zu "System" -> "Addon Module"';
$_ADDONLANG['FuseCP_confirmmigrate'] = 'Migration bestätigen';
$_ADDONLANG['FuseCP_confirmmigrate_long'] = 'Sind Sie sicher, dass Sie die Datenbanktabelle migrieren möchten? Es gibt keinen Weg zurück, um das alte Modul zu nutzen. Erstellen Sie bitte ein Backup dieser Tabelle bevor Sie mit der Migration beginnen. Dieser Vorgang kann nach Ihrer Bestätigung nicht mehr rückgängig gemacht werden!';
$_ADDONLANG['FuseCP_migrationrunning'] = 'Migrationsvorgang wird ausgeführt';
$_ADDONLANG['FuseCP_saverunning'] = 'Speichervorgang wird ausgeführt';
$_ADDONLANG['FuseCP_needfirstconfiguration'] = 'Dies ist die erste Konfiguration des Moduls. Bitte stellen Sie die entsprechenden Einstellungen ein und speichern Sie diese!';
$_ADDONLANG['FuseCP_norecordsfound'] = 'Keine Einträge gefunden';

$_ADDONLANG['FuseCP_configurableoptionname'] = 'Name der konfigurierbaren Option';
$_ADDONLANG['FuseCP_addonname'] = 'Addon-Name';
$_ADDONLANG['FuseCP_whmcs_id'] = 'WHMCS-ID';
$_ADDONLANG['FuseCP_fusecp_id'] = 'FuseCP-ID';
$_ADDONLANG['FuseCP_hidden'] = 'Versteckt';
$_ADDONLANG['FuseCP_delete'] = 'Löschen';
$_ADDONLANG['FuseCP_edit_configurable_option'] = 'Die Zuordnung der konfigurierbaren Optionen bearbeiten';
$_ADDONLANG['FuseCP_edit_addon_option'] = 'Die Zuordnung der Addons bearbeiten';
$_ADDONLANG['FuseCP_whmcs_id_tooltip'] = 'Geben Sie die WHMCS-ID aus der Datenbank ein.';
$_ADDONLANG['FuseCP_fusecp_id_tooltip'] = 'Geben Sie die FuseCP Addon-ID ein.';
$_ADDONLANG['FuseCP_is_ip_address'] = 'Ist IP-Addresse';
$_ADDONLANG['FuseCP_is_ip_address_tooltip'] = 'Soll eine neue dedizierte IP-Adresse in FuseCP zugeordnet werden?';
$_ADDONLANG['FuseCP_search_configurable'] = 'Suche nach konfigurierbare Option oder FuseCP-ID...';
$_ADDONLANG['FuseCP_show_only_assigned_conf'] = 'Nur zugewiesene konfigurierbare Optionen anzeigen';
$_ADDONLANG['FuseCP_search_addon'] = 'Suche nach Addon-Name oder FuseCP-ID...';
$_ADDONLANG['FuseCP_show_only_assigned_addons'] = 'Nur zugewiesene Addons anzeigen';

$_ADDONLANG['FuseCP_general_settings'] = 'Allgemeine Einstellungen';
$_ADDONLANG['FuseCP_setting_AddonsActive'] = 'Addon-Automatisierung aktiv';
$_ADDONLANG['FuseCP_setting_AddonsActive_tooltip'] = 'Addon-Provisionierung wird automatisiert werden, wenn diese Option aktiviert ist. Fügen Sie neue Einträge im Addon-Automatisierung-Tab hinzu, um diese Funktion nutzen zu können.';
$_ADDONLANG['FuseCP_setting_ConfigurableOptionsActive'] = 'Konfigurierbare Optionen aktiv';
$_ADDONLANG['FuseCP_setting_ConfigurableOptionsActive_tooltip'] = 'Provisionierung der konfigurierbaren Optionen wird automatisiert werden, wenn diese Option aktiviert ist und eine gültige Lizenz gefunden wurde. Fügen Sie neue Einträge im Konfigurierbare Optionen-Tab hinzu, um diese Funktion nutzen zu können.';
$_ADDONLANG['FuseCP_setting_SyncActive'] = 'Sync-Automatisierung aktiv';
$_ADDONLANG['FuseCP_setting_SyncActive_tooltip'] = 'Kundendetails werden automatisch mit dem jeweiligen FuseCP-Konto synchronisiert, wenn Änderungen in WHMCS vorgenommen wurden.';
$_ADDONLANG['FuseCP_setting_DeleteTablesOnDeactivate'] = 'DB-Tabellen beim Deaktivieren löschen';
$_ADDONLANG['FuseCP_setting_DeleteTablesOnDeactivate_tooltip'] = 'Tabellen in der Datenbank werden gelöscht, wenn das Modul deaktiviert wird und diese Option aktiviert ist.';
$_ADDONLANG['FuseCP_setting_WhmcsAdmin'] = 'WHMCS-Admin für API-Aufrufe';
$_ADDONLANG['FuseCP_setting_WhmcsAdmin_tooltip'] = 'WHMCS-Admin-Benutzer, der für die Aufrufe der internen API in den Kundenbereich-Scripts benutzt wird (z.B. zum Ändern des Dienste-Passworts).';

$_ADDONLANG['FuseCP_save_changes'] = 'Änderungen speichern';
$_ADDONLANG['FuseCP_cancel_changes'] = 'Änderungen verwerfen';
$_ADDONLANG['FuseCP_edit'] = 'Bearbeiten';
$_ADDONLANG['FuseCP_cancel'] = 'Abbrechen';
$_ADDONLANG['FuseCP_saving'] = 'Wird gespeichert';
