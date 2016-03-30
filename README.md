# CodeGenerater
一个可以根据模板生成代码的代码生成器

#list.txt
放在exe同目录，指定要生成的个性化文件的名称


内容如下

get_my_credit_rank_top_n_user

get_my_credit_record

get_invite_info


#templete_list.txt
放在exe同目录，指定有多少种模板以及文件生成后所在的目录


内容如下

Controllor,org/ls/bekind/

Service,org/ls/bekind/

ServiceImpl,org/ls/bekind/

Enum,org/ls/bekind/

Request,org/ls/bekind/

Response,org/ls/bekind/

Html,WebContent/WEB-INF/page/interface_test


#Templete（模板目录）
放在exe同目录下，目录内包含各种模板，对应templete_list.txt中的每一项

文件中可包含${source_data}${SourceData}${sourceData}三种变量对应三种命名方式

文件命名方式：templete_list.txt中的名称+Templete


目录中文件如下

ControllorTemplete.txt

EnumTemplete.txt


文件示例

package org.ls.bekind.client.inf.${source_data};

import org.ls.net.bean.BaseResponse;

public class ${SourceData}Response extends BaseResponse {

}
