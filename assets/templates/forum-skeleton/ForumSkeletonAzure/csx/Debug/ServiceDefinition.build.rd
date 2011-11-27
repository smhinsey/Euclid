<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ForumSkeletonAzure" generation="1" functional="0" release="0" Id="9d5ae209-dd7b-40ea-a139-ad531a061721" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ForumSkeletonAzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ForumSkeletonMvc:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/LB:ForumSkeletonMvc:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="ForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
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
      </channels>
      <maps>
        <map name="MapForumSkeletonMvc:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
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
          <role name="ForumSkeletonMvc" generation="1" functional="0" release="0" software="C:\Users\smhinsey\git\Euclid\assets\templates\forum-skeleton\ForumSkeletonAzure\csx\Debug\roles\ForumSkeletonMvc" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ForumSkeletonMvc&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ForumSkeletonMvc&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
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
    <implementation Id="aed8b152-27c9-4af8-9aa6-b24c0dd15858" ref="Microsoft.RedDog.Contract\ServiceContract\ForumSkeletonAzureContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="13b652c8-9c41-49ba-bab0-14ebf2e3c8b8" ref="Microsoft.RedDog.Contract\Interface\ForumSkeletonMvc:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/ForumSkeletonAzure/ForumSkeletonAzureGroup/ForumSkeletonMvc:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>