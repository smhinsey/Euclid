﻿<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ForumSkeletonAzure" generation="1" functional="0" release="0" Id="86cef275-83e4-496d-86e1-ff3526093f47" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ForumSkeletonAzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ForumSkeletonMvc:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/LB:ForumSkeletonMvc:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/LB:ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
        <inPort name="ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/LB:ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/MapCertificate|ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="ForumSkeletonMvcInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/MapForumSkeletonMvcInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:ForumSkeletonMvc:Endpoint1">
          <toPorts>
            <inPortMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint">
          <toPorts>
            <inPortMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc/Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapForumSkeletonMvcInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvcInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="ForumSkeletonMvc" generation="1" functional="0" release="0" software="C:\Users\smhinsey\git\Euclid\assets\templates\forum-skeleton\ForumSkeletonAzure\csx\Release\roles\ForumSkeletonMvc" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" protocol="tcp" portRanges="8172" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/SW:ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ForumSkeletonMvc&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ForumSkeletonMvc&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvcInstances" />
            <sCSPolicyFaultDomainMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvcFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="ForumSkeletonMvcFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="ForumSkeletonMvcInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="d5baf81f-85d0-46d0-8c06-b39b598df1c7" ref="Microsoft.RedDog.Contract\ServiceContract\ForumSkeletonAzureContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="c776ae8a-2447-4ed1-8fdb-574b896069f2" ref="Microsoft.RedDog.Contract\Interface\ForumSkeletonMvc:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="0908a8c9-f821-445a-a069-b866197d1121" ref="Microsoft.RedDog.Contract\Interface\ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="7ecd74d3-5fba-4191-841b-9641c6cdde3a" ref="Microsoft.RedDog.Contract\Interface\ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>