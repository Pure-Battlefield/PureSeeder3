<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="af3d047e-c047-4440-9242-9ed68416a37e" namespace="PureSeeder.Core.Configuration" xmlSchemaNamespace="urn:PureSeeder.Core.Configuration" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="PureConfigSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="PureConfig">
      <elementProperties>
        <elementProperty name="Servers" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="servers" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/af3d047e-c047-4440-9242-9ed68416a37e/ServerSettingsCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Settings" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="settings" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/af3d047e-c047-4440-9242-9ed68416a37e/AppSettingsCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="ServerSettingsCollection" collectionType="BasicMap" xmlItemName="server" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/af3d047e-c047-4440-9242-9ed68416a37e/Server" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="Server" documentation="A server to seed on">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/af3d047e-c047-4440-9242-9ed68416a37e/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Address" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="address" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/af3d047e-c047-4440-9242-9ed68416a37e/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="MinPlayers" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="minPlayers" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/af3d047e-c047-4440-9242-9ed68416a37e/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="MaxPlayers" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="maxPlayers" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/af3d047e-c047-4440-9242-9ed68416a37e/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="AppSettingsCollection" collectionType="BasicMap" xmlItemName="add" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/af3d047e-c047-4440-9242-9ed68416a37e/Setting" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="Setting">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/af3d047e-c047-4440-9242-9ed68416a37e/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Value" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="value" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/af3d047e-c047-4440-9242-9ed68416a37e/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>